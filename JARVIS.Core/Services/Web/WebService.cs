using System;
using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Server;

namespace JARVIS.Core.Services.Web
{
    public class WebService : Services.IService
    {

        RestServer Server;
        ServerSettings Settings;

        public Dictionary<string, IService> CallbackListeners = new Dictionary<string, IService>();
 
        // Pass by reference the config and hte ?
        public WebService(string host, string port)
        {
            Settings = new ServerSettings();
            Settings.Port = port;
            Settings.Host = host;
            Settings.UseHttps = false;

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

        public void HandleCallback(IHttpRequest request) { };

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
