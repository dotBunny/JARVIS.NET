using System;
using System.Collections.Generic;
using JARVIS.Core.Protocols.OAuth2;
using JARVIS.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Spotify
{
    public class StreamlabsService : IService
    {
        public const string ScopeAuthentication = "streamlabs-authenticate";

        // Settings Reference Keys
        const string SettingsEnabledKey = "Streamlabs.Enabled";
        const string SettingsClientIDKey = "Streamlabs.ClientID";
        const string SettingsClientSecretKey = "Streamlabs.ClientSecret";

        // Settings Values (pulled from DB)
        public bool Enabled { get; private set; }
        OAuth2Provider OAuth2 = new OAuth2Provider();


        public StreamlabsService()
        {
            // Initialize Settings
            Enabled = Server.Config.GetBool(SettingsEnabledKey);
            if (Enabled)
            {
                LoadSettings();
            }
        }


        void LoadSettings()
        {
            OAuth2 = new OAuth2Provider(GetName(),
                                        Server.Config.Get(SettingsClientIDKey),
                                        Server.Config.Get(SettingsClientSecretKey),
                                        "alerts.create alerts.write",
                                        "https://streamlabs.com/api/v1.0/authorize",
                                        "https://streamlabs.com/api/v1.0/token",
                                        ScopeAuthentication);
        }

        public string GetName()
        {
            return "Streamlabs";
        }


        public void Start()
        {
            if (!Enabled)
            {
                Log.Message("Streamlabs", "Unable to start as service is disabled.");
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
           
        }
    }
}