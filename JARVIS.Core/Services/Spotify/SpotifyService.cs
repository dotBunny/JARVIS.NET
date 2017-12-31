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
        SpotifyTrack LastTrack = new SpotifyTrack();



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
            WebAPI.Requests.TokenRequest tokenRequest = new WebAPI.Requests.TokenRequest(
                Code, 
                "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/", 
                State);

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + Strings.Base64Encode(ClientID + ":" + ClientSecret));

            // Get Response
            WebAPI.Responses.TokenResponse responseObject = tokenRequest.GetResponse();

            if (responseObject != null)
            {

                if (responseObject.ErrorCode != string.Empty)
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
                    ExpiresOn = DateTime.Now.AddSeconds(ExpiresIn);

                    // Flag we are good!
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
            WebAPI.Requests.RefreshTokenRequest tokenRequest = new WebAPI.Requests.RefreshTokenRequest(
                RefreshToken,
                State);

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + Strings.Base64Encode(ClientID + ":" + ClientSecret));

            // Get Response
            WebAPI.Responses.TokenResponse responseObject = tokenRequest.GetResponse();

            if (responseObject != null)
            {

                if (responseObject.ErrorCode != string.Empty)
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
            if (NextPoll < DateTime.Now) return;

            // Create Headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + Token);

            // Get Response
            var json = Shared.Web.GET(WebAPI.Responses.CurrentlyPlayingResponse.Endpoint, headers);

            // Process Response
            WebAPI.Responses.CurrentlyPlayingResponse responseObject = null;

            if (!string.IsNullOrEmpty(json))
            {
                responseObject = JsonConvert.DeserializeObject<WebAPI.Responses.CurrentlyPlayingResponse>(json);

                if (responseObject != null)
                {
                    if (responseObject.TrackID != LastTrack.ID)
                    {
                        LastTrack = responseObject.GetTrack();

                        // TODO :? Update something?
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

            Shared.Log.Message("Spotify", "Initiating authorization process ...");

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
            parameters.Add("scopes", "user-read-recently-played");
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
                Shared.Log.Message("Spotify", "Unable to start as service is disabled.");
                return;   
            }

            if (!Authenticated)
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

            if (!Authenticated && !string.IsNullOrEmpty(Token)) return;

            // Check our token
            if ( DateTime.Now > ExpiresOn ) {
                GetRefreshToken();
            }

            // To adjust polling speed?
            GetCurrentlyPlaying();


            Shared.Log.Message("Spotify", "POLLING SPOTIFY");
        }


    }

}
