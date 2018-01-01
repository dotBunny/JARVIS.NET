using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket
{
    public class SocketClient : ISocketClient
    {
        public string Host = "127.0.0.1";
        public int Port = 8081;
        public string EncryptionKey = "max";
        public bool Encryption = false;
        public JCP Protocol;

        public bool IsConnected {
            get { return Connection.Connected;  }
        }

        Shared.Services.Socket.SocketClient Connection;

        public SocketClient()
        {
            Host = Shared.Network.GetIPAddress(Host);

            // Create Client
            Connection = new Shared.Services.Socket.SocketClient();

            // Setup event handlers
            Connection.OnClosed += Connection_OnClosed;
            Connection.OnConnected += Connection_OnConnected;
            Connection.OnException += Connection_OnException;
            Connection.OnData += Connection_OnData;
        }

        void Connection_OnClosed(Sender session)
        {
            Shared.Log.Message("socket", "Disconnected from " + Host + ":" + Port.ToString());
        }
        void Connection_OnConnected(Sender session)
        {
            Shared.Log.Message("socket", "Connected to " + Host + ":" + Port.ToString());
        }
        void Connection_OnException(Sender session, Exception e)
        {
            Shared.Log.Error("socket", "An error occured. " + e.Message);
            Shared.Log.Error("socket", e.StackTrace);
        }

        // We do not need to handle sessions on the client because we know the session that is sending it, it must be the server.
        List<byte> Buffer = new List<byte>();
        void Connection_OnData(Sender session, byte[] data)
        {
            // Standardized data processor
            DataHandler.ProcessData(session, Buffer, data, Protocol, new CommandFactory());
        }


        public void Start()
        {
            // Initialize Protocol
            Protocol = new JCP(Encryption, EncryptionKey);
            if (Encryption)
            {
                Shared.Log.Message("socket", "Encryption Enabled");
            }
            else
            {
                Shared.Log.Message("socket", "Encryption DISABLED");
            }


            Host = Shared.Network.GetIPAddress(Host);
            Connection.Connect(Host, Port);
            Connection.Start();
        }

        public void Stop()
        {
            // Disconnect
            Connection.Stop();
        }


        public void Send(Instruction.OpCode type, Dictionary<string, string> parameters)
        {
            Packet p = new Packet(type, parameters);
            Shared.Log.Message("socket", "Sending " + p.GetOpCodes() + " to " + Host + ":" + Port.ToString());
            Connection.Sender.Send(Protocol.GetBytes(p));
        }
        public void Send(Packet packet)
        {
            Shared.Log.Message("socket", "Sending " + packet.GetOpCodes() + " to " + Host + ":" + Port.ToString());
            Connection.Sender.Send(Protocol.GetBytes(packet));
        }
        public void Send(Packet[] packets)
        {
            foreach(Packet p in packets){
                Shared.Log.Message("socket", "Sending " + p.GetOpCodes() + " to " + Host + ":" + Port.ToString());
            }
            Connection.Sender.Send(Protocol.GetBytes(packets));
        }

    }
}
