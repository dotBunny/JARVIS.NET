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
            Server.Port = Program.Config.Port;
        }

        ~WebService()
        {
            Server.Dispose();
            Server = null;
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
