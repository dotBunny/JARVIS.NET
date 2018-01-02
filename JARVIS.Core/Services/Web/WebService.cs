using System.Collections.Generic;
using Grapevine.Server;
using JARVIS.Core.Protocols.OAuth2;

namespace JARVIS.Core.Services.Web
{
    public class WebService : Services.IService
    {

        RestServer Server;
        ServerSettings Settings;

        public Dictionary<string, OAuth2Provider> CallbackListeners = new Dictionary<string, OAuth2Provider>();
 
        // Pass by reference the config and hte ?
        public WebService(string host, string port)
        {
            Settings = new ServerSettings
            {
                Port = port,
                Host = host,
                UseHttps = false
            };

            Server = new RestServer(Settings);
            Shared.Log.Message("web", "Attempting to listen on " + host + ":" + port);
        }

        ~WebService()
        {
            Server.Dispose();
            Server = null;
        }

        public string GetName()
        {
            return "Web";
        }

        public void Start()
        {
            Server.LogToConsole().Start();
        }

        public void Stop()
        {
            Server.Stop();   

        }

        public void Tick()
        {

        }
    }
}
