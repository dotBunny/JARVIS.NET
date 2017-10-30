using System;
using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket
{
    public class SocketClient 
    {
        public string Host = "127.0.0.1";
        public int Port = 8081;
        public string EncryptionKey = "max";
        public Protocol Parser;

        public bool IsConnected {
            get { return Connection.Connected;  }
        }

        GodSharp.Sockets.SocketClient Connection;

        public SocketClient()
        {
            // Initialize Protocol
            Parser = new Protocol(EncryptionKey);

            Host = Shared.Net.GetIPAddress(Host);
                  
            // Create Client
            Connection = new GodSharp.Sockets.SocketClient(Host, Port);

            // Setup event handlers
            Connection.OnClosed += Connection_OnClosed;
            Connection.OnConnected += Connection_OnConnected;
            Connection.OnException += Connection_OnException;
            Connection.OnData += Connection_OnData;
        }

        void Connection_OnClosed(GodSharp.Sockets.Sender session)
        {
            Shared.Log.Message("socket", "Disconnected from " + Host + ":" + Port.ToString());
        }
        void Connection_OnConnected(GodSharp.Sockets.Sender session)
        {
            Shared.Log.Message("socket", "Connected to " + Host + ":" + Port.ToString());
        }
        void Connection_OnException(GodSharp.Sockets.Sender session, Exception e)
        {
            Shared.Log.Error("socket", "An error occured. " + e.Message);
        }

        void Connection_OnData(GodSharp.Sockets.Sender session, byte[] data)
        {

            Protocol.Packet packet = Parser.GetPacket(data);
            Shared.Log.Message("socket", "Package Received -> " + packet.Command.ToString());

            // Factory Pattern
            ISocketCommand receivedCommand = CommandFactory.CreateCommand(packet.Command);

            // Move forward?
            if (receivedCommand.CanExecute())
            {
                receivedCommand.Execute(packet.Parameters);
            }
        }

        public void Start()
        {
            Host = Shared.Net.GetIPAddress(Host);
            Connection.Connect(Host, Port);
            Connection.Start();
        }

        public void Stop()
        {
            // Disconnect
            Connection.Stop();
        }

        public void Send(Shared.Services.Socket.Commands.Types type, Dictionary<string, string> parameters)
        {
            Shared.Log.Message("socket", "Sending " + type.ToString() + " to " + Host + ":" + Port.ToString());
            Connection.Sender.Send(Protocol.GetBytes(type, parameters));
        }

    }
}
