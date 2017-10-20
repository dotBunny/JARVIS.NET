using System;
using Grapevine.Server;

namespace JARVIS.Server.Services
{
    public class WebService : IService
    {
        Grapevine.Server.RestServer Server;

        public WebService()
        {
            Server = new RestServer();
            Server.Host = Program.Config.Host;
            Server.Port = Program.Config.WebPort.ToString();
            Shared.Log.Message("web", "Attempting to listen on " + Server.Host + ":" + Server.Port);
            Program.Services.Add(this);
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
