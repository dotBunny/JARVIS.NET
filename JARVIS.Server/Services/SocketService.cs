using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

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

        public void SendToAllSessions(string command, string arguments)
        {
            var sessions = Server.GetAllSessions();

            // Send to sessions
            foreach(AppSession session in sessions) {
                
                SendToSession(session, command, arguments);
            }
        }


        public static void SendToSession(AppSession session, string command, string arguments)
        {
            Shared.Log.Message("socket", "Sending to " + session.RemoteEndPoint + " [" + command.ToUpper() + Shared.Net.SocketDeliminator + arguments + Shared.Net.SocketTerminator + "]");
            session.Send(command.ToUpper() + Shared.Net.SocketDeliminator + arguments + Shared.Net.SocketTerminator);
        }


        static void HandleConnection(AppSession session)
        {
            Shared.Log.Message("socket", "New connection from " + session.RemoteEndPoint);

            SendToSession(session, "INFO", "Welcome to JARVIS.");
        }
    }
}
