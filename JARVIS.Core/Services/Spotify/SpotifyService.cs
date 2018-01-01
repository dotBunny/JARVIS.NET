using System;
using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
using JARVIS.Shared;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyService : IService
    {
        // Settings Reference Keys
        const string SettingsEnabledKey = "Spotify.Enabled";
        const string SettingsClientIDKey = "Spotify.ClientID";
        const string SettingsClientSecretKey = "Spotify.ClientSecret";

        // Settings Values (pulled from DB)
        public bool Enabled { get; private set; }
        string ClientID;
        string ClientSecret;


        public bool Authenticated = false;


        string State;
        string Code = string.Empty;
        string Token = string.Empty;
        string RefreshToken = string.Empty;
        string Scope = string.Empty;
        int ExpiresIn = 0;
        DateTime ExpiresOn;
        DateTime NextPoll;

        // Track information
        public SpotifyTrack LastTrack = new SpotifyTrack();
        public EventHandler<SpotifyTrack> NewTrackEvent;



        public SpotifyService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if ( Enabled ) {
                ClientID = Server.Config.Get(SettingsClientIDKey);
                ClientSecret = Server.Config.Get(SettingsClientSecretKey);
            }
        }


        public string GetName()
        {
            return "Spotify";
        }

        public void HandleCallbackAsync(IHttpRequest request)
        {
            string state = request.QueryString.GetValue("state", string.Empty);

            // Stage 1 
            if ( Code == string.Empty ) {
                Code = request.QueryString.GetValue("code", string.Empty);

                if ( Code != string.Empty ) 
                {
                    GetToken();
                }
            }
        }


        void GetToken()
        {
            // Create our token request
            var tokenRequest = new WebAPI.Requests.TokenRequest
            {
                Code = Code,
                RedirectURI = "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/",
                State = State
            };

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + Strings.Base64Encode(ClientID + ":" + ClientSecret));

            // Get Response
            var responseObject = tokenRequest.GetResponse();

            if (responseObject != null)
            {

                if (!string.IsNullOrEmpty(responseObject.ErrorCode))
                {
                    Log.Error("Spotify", "An error occured (" + responseObject.ErrorCode + ") while getting the token. " + responseObject.ErrorDescription);
                    Authenticated = false;
                }
                else
                {
                    Token = responseObject.AccessToken;
                    RefreshToken = responseObject.RefreshToken;
                    ExpiresIn = responseObject.ExpiresInSeconds;
                    Scope = responseObject.Scope;

                    NextPoll = DateTime.Now;
                    ExpiresOn = NextPoll.AddSeconds(ExpiresIn);

                    // Flag we are good!
                    Log.Message("Spotify", "Authentication Successful. (" + Token + ")");
                    Authenticated = true;
                }
            }
            else
            {
                Authenticated = false;
                Log.Error("Spotify", "Spotify failed to get the token. NULL Response Object.");
            }
          
        }

        void GetRefreshToken()
        {
            Log.Message("Spotify", "Refreshing Token");

            var tokenRequest = new WebAPI.Requests.RefreshTokenRequest(RefreshToken, State);

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + Strings.Base64Encode(ClientID + ":" + ClientSecret));

            // Get Response
            var responseObject = tokenRequest.GetResponse();

            if (responseObject != null)
            {

                if (!string.IsNullOrEmpty(responseObject.ErrorCode))
                {
                    Log.Error("Spotify", "An error occured (" + responseObject.ErrorCode + ") while refreshing the token. " + responseObject.ErrorDescription);
                    Authenticated = false;
                }
                else
                {
                    Token = responseObject.AccessToken;
                    ExpiresIn = responseObject.ExpiresInSeconds;
                    Scope = responseObject.Scope;
                    ExpiresOn = DateTime.Now.AddSeconds(ExpiresIn);

                    // Flag we are good!
                    Authenticated = true;
                }
            }
            else
            {
                Authenticated = false;
                Log.Error("Spotify", "Spotify failed to refresh the token. NULL Response Object.");
            }
           
        }

        void GetCurrentlyPlaying()
        {
            if (NextPoll > DateTime.Now) return;

            // Create Headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + Token);

            // Get Response
            var json = Shared.Web.GetJSON(WebAPI.Responses.CurrentlyPlayingResponse.Endpoint, headers);

            // Process Response
            WebAPI.Responses.CurrentlyPlayingResponse responseObject = null;

            if (!string.IsNullOrEmpty(json))
            {
                responseObject = JsonConvert.DeserializeObject<WebAPI.Responses.CurrentlyPlayingResponse>(json);

                if (responseObject != null)
                {
                    if ( responseObject.Error != null)
                    {
                        Log.Message("Spotify", "An error (" + responseObject.Error.Code + ") occured while trying to poll the currently playing track. " + responseObject.Error.Description);
                    }
                    else if (responseObject.TrackID != LastTrack.ID)
                    {
                        LastTrack = responseObject.GetTrack();

                        // Call any subscribers
                        if (NewTrackEvent != null)
                        {
                            NewTrackEvent(this, LastTrack);
                        }

                        Dictionary<string, string> parameters = new Dictionary<string, string>();
                        parameters.Add("filename", "Spotify.txt");
                        parameters.Add("content", LastTrack.ToInfoString());
                        Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters);
                        parameters["filename"] = "Spotify_Artist.txt";
                        parameters["content"] = LastTrack.Artist;
                        Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters);
                        parameters["filename"] = "Spotify_Track.txt";
                        parameters["content"] = LastTrack.Track;
                        Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters);
                        parameters["filename"] = "Spotify_URL.txt";
                        parameters["content"] = LastTrack.TrackURL;
                        Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters);

                        // TODO: Send image data to be saved
                        if (!string.IsNullOrEmpty(LastTrack.ImageURL))
                        {
                            LastTrack.ImageData = Shared.Web.GetBytes(LastTrack.ImageURL);
                            parameters["filename"] = "Spotify_TrackImage.jpg";
                            parameters["content"] = Convert.ToBase64String(LastTrack.ImageData);

                            Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.BINARY_FILE, parameters);

                        }

                        Log.Message("Spotify", LastTrack.ToString());
                    }
                }
                else
                {
                    Log.Error("Spotify", "Spotify failed to update currently playing. NULL Response Object.");
                }
            }
            else
            {
                Log.Error("Spotify", "Spotify failed to update currently playing. No Response.");
            }

            // Poll every 10 seconds?
            NextPoll = DateTime.Now.AddSeconds(10);
        }

        void Authorize()
        {
            Authenticated = false;

            Token = string.Empty;
            Code = string.Empty;
            RefreshToken = string.Empty;
            Scope = "";
            ExpiresIn = 0;

            Log.Message("Spotify", "Initiating authorization process ...");

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // Create random key that is used later to make sure this is the one that processes it
            State = Guid.NewGuid().ToString();

            // These will be split and used in the function itself
            parameters.Add("endpoint", "https://accounts.spotify.com/authorize/?response_type=code");
            parameters.Add("title", "Spotify Authentication");
            parameters.Add("message", "JARVIS needs to you to authenticate with your Spotify account for it to be able to poll data.");

            // Data passed to each call (with the execption of the token)
            parameters.Add("client_id", ClientID);
            parameters.Add("client_secret", ClientSecret);

            // Ask for a lot of perms
            parameters.Add("scope", "playlist-read-private playlist-read-collaborative user-read-playback-state user-modify-playback-state user-read-currently-playing user-read-recently-played");
            parameters.Add("state", State);
            parameters.Add("redirect_uri", "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/");

            // Add to listeners
            Server.Web.CallbackListeners.Add(State, this);

            // Only send to our client based OAUTH manager
            // TODO: Make this so it requires logged in(false->True)
            Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.OAUTH_REQUEST, parameters, false);
        }
      

        public void Start()
        {
            if (!Enabled) 
            {
                Log.Message("Spotify", "Unable to start as service is disabled.");
                return;   
            }

            // TODO:    Switch this to use the actual authorized user count of the server
            //          not implemented at this time though. Need to fix user authentication (NEXT!)
            if (!Authenticated && Server.Socket.BufferCount > 0)
            {
                Authorize();
            }
        }

        public void Stop()
        {
            Authenticated = false;
        }

        public void Tick()
        {

            // Don't bother if we haven't authenticated and dont have a token
            if (!Authenticated || string.IsNullOrEmpty(Token)) return;

            // Check our token
            if ( DateTime.Now >= ExpiresOn ) {
                GetRefreshToken();
            }

            // To adjust polling speed?
            GetCurrentlyPlaying();
        }
    }
}