using System;
using Grapevine.Server;

namespace JARVIS.Core.Services.Web
{
    public class WebService : Services.IService
    {

        Grapevine.Server.RestServer Server;

        // Pass by reference the config and hte ?
        public WebService(string host, string port)
        {
            Server = new RestServer();
            Server.Host = host;
            Server.Port = port;
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
    }
}
