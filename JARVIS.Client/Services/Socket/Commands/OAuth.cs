using System;
using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using JARVIS.Shared;

namespace JARVIS.Client.Services.Socket.Commands
{
    public class OAuth : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return Settings.FeatureFileOutputs;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // Build URI - errors if we dont have it


            Uri endpoint = new Uri(parameters["endpoint"]);
            parameters.Remove("endpoint");


            string title = "OAuth Request";
            if (parameters.ContainsKey("title"))
            {;
                title = parameters["title"];
                parameters.Remove("title");
            }

            string message = "A service requires you to login for proper use.";
            if (parameters.ContainsKey("message"))
            {
                message = parameters["message"];
                parameters.Remove("message");
            }

            // Add left over parameters
            foreach(KeyValuePair<string,string> s in parameters)
            {
                endpoint = endpoint.AddQuery(s.Key, s.Value);
            }

            OAuthNotification notification = new OAuthNotification(endpoint.ToString(), title, message);
            if (parameters.ContainsKey("state"))
            {
                notification.State = parameters["state"];
            }
            Log.Notifier.Notify(notification);
        }

       

    }
}
