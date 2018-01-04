using System;
using System.Collections.Generic;
using JARVIS.Core.Protocols.OAuth2;
using JARVIS.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.WebUtilities;

namespace JARVIS.Core.Services.Streamlabs
{
    public class StreamlabsService : IService
    {
        public enum AlertType
        {
            Follow,
            Subscription,
            Donation,
            Host
        }
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
                                        ScopeAuthentication, false);
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

            if (!OAuth2.CheckToken() && Server.Services.GetService<Socket.SocketService>().AuthenticatedUserCount > 0)
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

        string AlertTypeToString(AlertType alertType)
        {
            switch(alertType)
            {
                case AlertType.Follow:
                    return "follow";
                case AlertType.Subscription:
                    return "subscription";
                case AlertType.Host:
                    return "host";
                default:
                    return "donation";
            }

        }


        public void Alert(string alertMessage, string alertMilliseconds = "2000", string alertImage = "", AlertType alertType = AlertType.Donation )
        {
            if (!Enabled) { return; }

            // Check token / authentication
            if (!OAuth2.CheckToken()) { return; }

            // Create URI
            string endpoint = QueryHelpers.AddQueryString("https://streamlabs.com/api/v1.0/alerts", 
                                                          new Dictionary<string, string>()
            {
                { "type", AlertTypeToString(alertType) },
                { "image_href", alertImage },
                { "message", alertMessage },
                { "duration", alertMilliseconds }
            });

            // Create Headers
            var json = Shared.Web.PostJSON(endpoint, string.Empty, new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + OAuth2.Token }
            });

            Protocols.OAuth2.Responses.StatusMessage responseObject = JsonConvert.DeserializeObject<Protocols.OAuth2.Responses.StatusMessage>(json);

            if ( responseObject != null ) {
                if (responseObject.Successful) {
                    
                } else {
                    Log.Error("Streamlabs", "An error (" + responseObject.Code + ") occured. " + responseObject.Description);
                }
            } else {
                Log.Error("Streamlabs", "An error occured. NULL response object.");
            }
        }

    }
}