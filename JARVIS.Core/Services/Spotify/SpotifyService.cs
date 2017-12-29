using System;
using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
//using Discord.Rest;

using SpotifyAPI;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;

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

        //DiscordRestConfig Config;
        //DiscordRestClient Client;
        //RestGuild Guild;



        SpotifyAPI.Web.SpotifyWebAPI API;

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

        public void HandleCallback(IHttpRequest request)
        {
            string state = request.QueryString.GetValue<string>("state", string.Empty);

            // Stage 1 
            if ( Code == string.Empty ) {
                Code = request.QueryString.GetValue<string>("state", string.Empty);

                if ( Code != string.Empty ) 
                {
                    // Launch ASYNC Request?

                }
            }
           
        }

        void Authorize()
        {
            Authenticated = false;
            Token = string.Empty;
            Code = string.Empty;

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

        void GetToken()
        {
         //   parameters.Add("uri_token", "https://accounts.spotify.com/api/token/?grant_type=authorization_code");
        }

      

        public void Start()
        {
            if (!Enabled) 
            {
                Shared.Log.Message("Spotify", "Unable to start as service is disabled.");
                return;   
            }
                
            API = new SpotifyAPI.Web.SpotifyWebAPI();

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
            Shared.Log.Message("Spotify", "POLLING SPOTIFY");
        }

    }

}
