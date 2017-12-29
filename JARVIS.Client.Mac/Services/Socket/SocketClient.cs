using System;
using System.Collections.Generic;
using AppKit;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Mac.Services.Socket
{
    public class SocketClient
    {
        public JCP Protocol;

        public bool IsConnected
        {
            get { return Connection.Connected; }
        }

        Shared.Services.Socket.SocketClient Connection;

        NSMenuItem MenuConnect;
        NSMenuItem MenuDisconnect;

        public SocketClient(NSMenuItem connect, NSMenuItem disconnect)
        {
            // Assign menu items
            MenuConnect = connect;
            MenuDisconnect = disconnect;

            // Create Client
            Connection = new Shared.Services.Socket.SocketClient(Settings.ServerAddress, Settings.ServerPort);

            // Setup event handlers
            Connection.OnClosed += Connection_OnClosed;
            Connection.OnConnected += Connection_OnConnected;
            Connection.OnException += Connection_OnException;
            Connection.OnData += Connection_OnData;
        }

        void Connection_OnClosed(Sender session)
        {

            MenuConnect.Enabled = true;
            MenuDisconnect.Enabled = false;

            Shared.Log.Message("socket", "Disconnected from " + Settings.ServerAddress + ":" + Settings.ServerPort.ToString());
        }
        void Connection_OnConnected(Sender session)
        {

            MenuConnect.Enabled = false;
            MenuDisconnect.Enabled = true;

            Shared.Log.Message("socket", "Connected to " + Settings.ServerAddress + ":" + Settings.ServerPort.ToString());
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
            // Adding data to our internal buffer
            Buffer.AddRange(data);

            int terminator = Buffer.IndexOf(JCP.TransmissionTerminator);
            while (terminator != -1)
            {

                Packet[] packets = Protocol.GetPackets(Buffer.GetRange(0, terminator).ToArray());
                foreach (Packet p in packets)
                {
                    foreach (Instruction i in p.GetInstructions())
                    {
                        Shared.Log.Message("socket", "Instruction Received -> " + i.Operation.ToString());

                        // Factory Pattern
                        ISocketCommand receivedCommand = CommandFactory.CreateCommand(i.Operation, this);

                        // Move forward?
                        if (receivedCommand.CanExecute())
                        {
                            receivedCommand.Execute(session, i.Parameters);
                        }
                    }
                }

                Buffer.RemoveRange(0, terminator + 1);

                // Look again
                terminator = Buffer.IndexOf(JCP.TransmissionTerminator);
            }

        }


        public void Start()
        {
            // Initialize Protocol
            Protocol = new JCP(Settings.EncryptionUseEncryptedProtocol, Settings.EncryptionServerEncryptionKey);
            if (Settings.EncryptionUseEncryptedProtocol)
            {
                Shared.Log.Message("socket", "Encryption Enabled");
            }
            else
            {
                Shared.Log.Message("socket", "Encryption DISABLED");
            }


           
            Connection.Connect(Settings.ServerAddress, Settings.ServerPort);
            Connection.Start();

            MenuConnect.Enabled = false;
        }

        public void Stop()
        {
            // Disconnect
            Connection.Stop();
        }


        public void Send(Instruction.OpCode type, Dictionary<string, string> parameters)
        {
            Packet p = new Packet(type, parameters);
            Shared.Log.Message("socket", "Sending " + p.GetOpCodes() + " to " + Settings.ServerAddress + ":" + Settings.ServerPort.ToString());
            Connection.Sender.Send(Protocol.GetBytes(p));
        }
        public void Send(Packet packet)
        {
            Shared.Log.Message("socket", "Sending " + packet.GetOpCodes() + " to " + Settings.ServerAddress + ":" + Settings.ServerPort.ToString());
            Connection.Sender.Send(Protocol.GetBytes(packet));
        }
        public void Send(Packet[] packets)
        {
            foreach (Packet p in packets)
            {
                Shared.Log.Message("socket", "Sending " + p.GetOpCodes() + " to " + Settings.ServerAddress + ":" + Settings.ServerPort.ToString());
            }
            Connection.Sender.Send(Protocol.GetBytes(packets));
        }

    }
}
