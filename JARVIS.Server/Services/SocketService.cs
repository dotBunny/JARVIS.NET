using System;
using SuperSocket.SocketBase;

namespace JARVIS.Server.Services
{
    public class SocketService : IService
    {
        private AppServer Server;

        public SocketService()
        {
            Server = new AppServer();

            if (!Server.Setup(Program.Config.SocketPort))
            {
                Shared.Log.Error(GetName(), "Unable to setup socket service.");
                return;
            }

            Program.Services.Add(this);
        }

        ~SocketService()
        {
            Server.Dispose();
        }

        public string GetName() 
        {
            return "Socket";   
        }
        public void Start()
        {
            if (!Server.Start())
            {
                Shared.Log.Error(GetName(), "Unable to start socket service.");
                return;
            }

            // Setup handlers
            Server.NewSessionConnected += new SessionHandler<AppSession>(HandleConnection);
        }

        public void Stop()
        {
            Server.Stop();
        }

        static void HandleConnection(AppSession session)
        {
            session.Send("Welcome to JARVIS.");
        }
    }
}
