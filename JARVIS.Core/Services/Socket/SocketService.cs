using System;
using System.Collections.Generic;
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
            Server.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(HandleRequestReceived);
        }


        public void Stop()
        {
            Server.Stop();
        }

        public void SendToAllSessions(Shared.Services.Socket.Commands.Types type, string body, Dictionary<string, string> arguments)
        {
            var sessions = Server.GetAllSessions();

            // Send to sessions
            foreach(AppSession session in sessions) 
            {
                SendToSession(session, type, body, arguments);
            }
        }


        public static void SendToSession(AppSession session, Shared.Services.Socket.Commands.Types type, string body, Dictionary<string, string> arguments)
        {
            Shared.Log.Message("socket", "Sending " + type.ToString() + " to " + session.RemoteEndPoint);
            session.Send(Shared.Services.Socket.SocketFilter.GetSocketString(type, body, arguments));
        }


        static void HandleConnection(AppSession session)
        {
            Shared.Log.Message("socket", "New connection from " + session.RemoteEndPoint);

            SendToSession(session, 
                          Shared.Services.Socket.Commands.Types.INFO, 
                          string.Empty, 
                          new Dictionary<string, string>() { { "message", "Welcome to JARVIS." } });

            // Request AUTH
            SendToSession(session, 
                          Shared.Services.Socket.Commands.Types.AUTH, 
                          string.Empty, 
                          null);
        }
        static void HandleDisconnect(AppSession session, CloseReason reason)
        {
            Shared.Log.Message("socket", "Closing connection from " + session.RemoteEndPoint + " for " + reason.ToString());
        }

        static void HandleRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            //switch (requestInfo.Key.ToUpper())
            //{
            //    case ("ECHO"):
            //        session.Send(requestInfo.Body);
            //        break;

            //    case ("ADD"):
            //        session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
            //        break;

            //    case ("MULT"):

            //        var result = 1;

            //        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
            //        {
            //            result *= factor;
            //        }

            //        session.Send(result.ToString());
            //        break;
            //}
        }
    }
}
