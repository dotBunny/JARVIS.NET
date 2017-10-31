﻿using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket
{
    public class SocketService : IService
    {
        // TODO: Add ability to sub to events that get rebroadcasted
        // TODO: Add REAUTH/AUTH
        SocketServer Server;

        public SocketService(string Host = "127.0.0.1", int SocketPort = 1331)
        {
            Server = new SocketServer();

            // Setup handlers
            Server.OnConnected += Server_OnConnected;
            Server.OnClosed += Server_OnClosed;
            Server.OnException += Server_OnException;
            Server.OnData += Server_OnData;

            // Set port
            Server.Host = Host;
            Server.Port = SocketPort;
        }

        ~SocketService()
        {
            Server = null;
        }

        void Server_OnClosed(Sender session)
        {
            Shared.Log.Message("socket", "Closing connection from " + session.RemoteEndPoint);
        }

        void Server_OnConnected(Sender session)
        {
            Shared.Log.Message("socket", "New connection from " + session.RemoteEndPoint);
            SendToSession(session, Shared.Services.Socket.Commands.Types.INFO, new Dictionary<string, string> { { "message", "Welcome to JARVIS." } });
            SendToSession(session, Shared.Services.Socket.Commands.Types.AUTH);
        }

        void Server_OnException(Sender session, System.Exception e)
        {
            Shared.Log.Message("socket", "Exception from " + session.RemoteEndPoint + " of " + e.Message);
        }

        void Server_OnData(Sender session, byte[] data)
        {
            Shared.Log.Message("request", "from " + session.RemoteEndPoint);
        }

        public string GetName() 
        {
            return "Socket";   
        }

        public void Start()
        {
            Server.Listen();
            Server.Start();
            Shared.Log.Message("socket", "Listening on " + Server.Port.ToString());
        }

        public void Stop()
        {
            Server.Stop();
        }

        public void SendToAllSessions(Shared.Services.Socket.Commands.Types type, Dictionary<string, string> arguments)
        {
            // Send to sessions
            foreach(Sender session in Server.Clients)
            {
                SendToSession(session, type, arguments);
            }
        }

        public static void SendToSession(Sender session, Shared.Services.Socket.Commands.Types type)
        {
            SendToSession(session, type, new Dictionary<string, string> { });
        }

        public static void SendToSession(Sender session, Shared.Services.Socket.Commands.Types type, Dictionary<string, string> arguments)
        {
            Shared.Log.Message("socket", "Sending " + type.ToString() + " to " + session.RemoteEndPoint);

            // Create package
            byte[] data = Protocol.GetBytes(type, arguments);

            // TODO: Check for fail?
            session.Send(data);
        }

    }
}
