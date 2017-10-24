using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Core.Services.Socket
{
    public class SocketService : Core.Services.IService
    {

        private AppServer Server;
        public System.Collections.Generic.List<AppSession> AuthenticatedSessions = new System.Collections.Generic.List<AppSession>();

        public SocketService(int SocketPort = 8081)
        {
            Server = new AppServer();

            if (!Server.Setup(SocketPort))
            {
                Shared.Log.Error(GetName(), "Unable to setup socket service.");
                return;
            }
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
            Server.SessionClosed += new SessionHandler<AppSession, CloseReason>(HandleDisconnect);
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
            SendToSession(session, "INFO", "message" + Shared.Net.SocketDeliminator + "Welcome to JARVIS.");

            // Request AUTH
            SendToSession(session, "AUTH", "");
        }
        static void HandleDisconnect(AppSession session, CloseReason reason)
        {
            Shared.Log.Message("socket", "Closing connection from " + session.RemoteEndPoint + " for " + reason.ToString());
        }
    }
}
