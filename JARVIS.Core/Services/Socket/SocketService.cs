using System;
using System.Collections.Generic;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Core.Services.Socket
{
    public class SocketService : IService
    {


        public List<AppSession> AuthenticatedSessions = new List<AppSession>();
        int Port = 8081;
        AppServer Server;

        public SocketService(int SocketPort = 8081)
        {
            Server = new AppServer();

            // Setup handlers

            // new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);
            Server.NewSessionConnected += HandleConnection;
            Server.SessionClosed += HandleDisconnect;
            Server.NewRequestReceived += HandleRequestReceived;


            Port = SocketPort;

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
            Shared.Log.Message("socket", "Listening on " + Port.ToString());
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

            // Create package
            byte[] data = Shared.Services.Socket.Protocol.GetBytes(type, body, arguments);

            // Send and check for failure
            if(!session.TrySend(data, 0, data.Length))
            {
                Shared.Log.Error("socket", "Failed to send " + type.ToString() + " to " + session.RemoteEndPoint);
            }
        }

        static void HandleConnection(AppSession session)
        {
            Shared.Log.Message("socket", "New connection from " + session.RemoteEndPoint);

            SendToSession(session, 
                          Shared.Services.Socket.Commands.Types.INFO, 
                          string.Empty, 
                          new Dictionary<string, string> { { "message", "Welcome to JARVIS." } });

            // Request AUTH
            SendToSession(session, 
                          Shared.Services.Socket.Commands.Types.AUTH, 
                          string.Empty, 
                          new Dictionary<string, string> { });
        }
        static void HandleDisconnect(AppSession session, CloseReason reason)
        {
            Shared.Log.Message("socket", "Closing connection from " + session.RemoteEndPoint + " for " + reason);
        }

        static void HandleRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Shared.Log.Message("request", "from " + session.RemoteEndPoint);
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
