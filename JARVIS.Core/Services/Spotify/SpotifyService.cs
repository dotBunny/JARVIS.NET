using System;
using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
using RestSharp;

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
            string state = request.QueryString.GetValue<string>("state", string.Empty);

            // Stage 1 
            if ( Code == string.Empty ) {
                Code = request.QueryString.GetValue<string>("state", string.Empty);

                if ( Code != string.Empty ) 
                {
                    GetToken();
                }
            }
        }

        void GetToken()
        {

            RestClient client = new RestClient("https://accounts.spotify.com");

            RestRequest request = new RestRequest(JSON.TokenResponse.Endpoint);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", Code);
            request.AddParameter("redirect_uri", "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/");

            // Add Authorization Header
            request.AddHeader("Authorization", "Basic: " + Shared.Strings.Base64Encode(ClientID + ":" + ClientSecret));

            var response = client.Execute<JSON.TokenResponse>(request);

            // TODO: Fails on execution (returning error code 0 thats it);

            if ( response.IsSuccessful ) {
                JSON.TokenResponse responseObject = response.Data;

                if (!string.IsNullOrEmpty(responseObject.ErrorCode))
                {
                    Shared.Log.Error("Spotify", "(" + responseObject.ErrorCode + ") " + responseObject.ErrorDescription);
                    Authenticated = false;
                }
                else
                {
                    Token = responseObject.AccessToken;
                    RefreshToken = responseObject.RefreshToken;
                    ExpiresIn = responseObject.ExpiresInSeconds;
                    Scope = responseObject.Scope;
                    ExpiresOn = DateTime.Now.AddSeconds(ExpiresIn);

                    Authenticated = true;
                }
            } else {
                Authenticated = false;

                Shared.Log.Error("Spotify", "Spotify failed to get the token. (" + response.StatusCode + ") " + response.StatusDescription);
            }
        }

        void GetRefreshToken()
        {
            RestClient client = new RestClient("https://accounts.spotify.com");

            RestRequest request = new RestRequest(JSON.TokenResponse.Endpoint);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", RefreshToken);

            // Add Authorization Header
            request.AddHeader("Authorization", "Basic: " + Shared.Strings.Base64Encode(ClientID + ":" + ClientSecret));

            var response = client.Execute<JSON.TokenResponse>(request);

            if (response.IsSuccessful)
            {
                JSON.TokenResponse responseObject = response.Data;

                if (!string.IsNullOrEmpty(responseObject.ErrorCode))
                {
                    Shared.Log.Error("Spotify", "("+responseObject.ErrorCode+ ") " + responseObject.ErrorDescription);
                }
                else
                {
                    Token = responseObject.AccessToken;
                    ExpiresIn = responseObject.ExpiresInSeconds;
                    Scope = responseObject.Scope;
                    ExpiresOn = DateTime.Now.AddSeconds(ExpiresIn);
                }

            }
            else
            {
                Authenticated = false;
                Shared.Log.Error("Spotify", "Spotify failed to get the refresh token.");
            }
        }

        void GetCurrentlyPlaying()
        {
            if (NextPoll < DateTime.Now) return;

            RestClient client = new RestClient("https://api.spotify.com");
            RestRequest request = new RestRequest(JSON.CurrentlyPlayingResponse.Endpoint);
            request.AddHeader("Authorization", "Bearer: " + Token);

            var response = client.Execute<JSON.CurrentlyPlayingResponse>(request);

            if (response.IsSuccessful)
            {
                JSON.CurrentlyPlayingResponse responseObject = response.Data;


                if ( responseObject.TrackID != LastTrack.ID ) {
                    LastTrack = responseObject.GetTrack();

                    // TODO :? Update something?
                    Shared.Log.Message("Spotify", LastTrack.ToString());
                }

            }
            else
            {
                Shared.Log.Error("Spotify", "Spotify failed to update currently playing.");
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
