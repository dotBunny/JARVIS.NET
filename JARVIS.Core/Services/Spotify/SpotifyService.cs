using System;
using System.Collections.Generic;
using JARVIS.Core.Protocols.OAuth2;
using JARVIS.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Spotify
{
    public class SpotifyService : IService
    {
        public const string ScopeAuthentication = "spotify-authenticate";
        public const string ScopeOutput = "spotify-output";


        // Settings Reference Keys
        const string SettingsEnabledKey = "Spotify.Enabled";
        const string SettingsClientIDKey = "Spotify.ClientID";
        const string SettingsClientSecretKey = "Spotify.ClientSecret";

        // Settings Values (pulled from DB)
        public bool Enabled { get; private set; }
        OAuth2Provider OAuth2 = new OAuth2Provider();

 
        DateTime NextPoll;

        // Track information
        public SpotifyTrack LastTrack = new SpotifyTrack();
        public EventHandler<SpotifyTrack> NewTrackEvent;



        public SpotifyService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if ( Enabled ) {
                LoadSettings();
            }
        }

        void LoadSettings()
        {
            OAuth2 = new OAuth2Provider(GetName(),
                                        Server.Config.Get(SettingsClientIDKey),
                                        Server.Config.Get(SettingsClientSecretKey),
                                        "playlist-read-private playlist-read-collaborative user-read-playback-state user-modify-playback-state user-read-currently-playing user-read-recently-played",
                                        "https://accounts.spotify.com/authorize/?response_type=code",
                                        "https://accounts.spotify.com/api/token",
                                        "https://accounts.spotify.com/api/token", ScopeAuthentication);
        }

        public string GetName()
        {
            return "Spotify";
        }

        void GetCurrentlyPlaying()
        {
            if (NextPoll > DateTime.Now) return;

            // Check token / authentication
            if (!OAuth2.IsValid()) { return;  }

            // Create Headers
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + OAuth2.Token }
            };

            // Get Response
            var json = Shared.Web.GetJSON(WebAPI.Responses.CurrentlyPlayingResponse.Endpoint, headers);

            // Process Response
            WebAPI.Responses.CurrentlyPlayingResponse responseObject = null;

            if (!string.IsNullOrEmpty(json) && json != "")
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
                        NewTrackEvent?.Invoke(this, LastTrack);

                        Dictionary<string, string> parameters = new Dictionary<string, string>
                        {
                            { "filename", "Spotify.txt" },
                            { "content", LastTrack.ToInfoString() }
                        };


                        Socket.SocketService socket = Server.Services.GetService<Socket.SocketService>();
                        socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters, true, ScopeOutput);

                        parameters["filename"] = "Spotify_Artist.txt";
                        parameters["content"] = LastTrack.Artist;
                        socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters, true, ScopeOutput);

                        parameters["filename"] = "Spotify_Track.txt";
                        parameters["content"] = LastTrack.Track;
                        socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters, true, ScopeOutput);

                        parameters["filename"] = "Spotify_URL.txt";
                        parameters["content"] = LastTrack.TrackURL;
                        socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.TEXT_FILE, parameters, true, ScopeOutput);

                        // TODO: Send image data to be saved
                        if (!string.IsNullOrEmpty(LastTrack.ImageURL) && LastTrack.ImageURL != "")
                        {
                            LastTrack.ImageData = Shared.Web.GetBytes(LastTrack.ImageURL);
                            parameters["filename"] = "Spotify_TrackImage.jpg";
                            parameters["content"] = Convert.ToBase64String(LastTrack.ImageData);

                            socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.BINARY_FILE, parameters, true, ScopeOutput);

                        }

                        // Save track to database
                        LastTrack.SaveToDatabase();

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


      
        public void Start()
        {
            if (!Enabled) 
            {
                Log.Message("Spotify", "Unable to start as service is disabled.");
                return;   
            }

            if (!OAuth2.IsValid() && Server.Services.GetService<Socket.SocketService>().AuthenticatedUserCount > 0)
            {
                OAuth2.Login();
            }
        }

        public void Stop()
        {
            OAuth2.Reset();
        }

        public void Tick()
        {
            // Don't bother if we haven't authenticated and dont have a token
            if (!Enabled || !OAuth2.IsValid()) return;

            // To adjust polling speed?
            GetCurrentlyPlaying();
        }
    }
}