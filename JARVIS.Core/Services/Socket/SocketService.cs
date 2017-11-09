using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using JARVIS.Shared.Protocol;

namespace JARVIS.Core.Services.Socket
{
    public class SocketService : IService
    {
        Dictionary<Sender, List<byte>> Buffers = new Dictionary<Sender, List<byte>>();
        // TODO: Add ability to sub to events that get rebroadcasted
        // TODO: Add REAUTH/AUTH
        SocketServer Server;
        JCP Protocol;

        public SocketService(string Host, int SocketPort, bool socketEncryption, string socketKey)
        {
            Server = new SocketServer();

            // Setup Parser
            Protocol = new JCP(socketEncryption, socketKey);

            if (socketEncryption)
            {
                Shared.Log.Message("socket", "Encryption Enabled");
            }
            else
            {
                Shared.Log.Message("socket", "Encryption DISABLED");
            }


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


            Buffers.Remove(session);
        }

        void Server_OnConnected(Sender session)
        {
            Shared.Log.Message("socket", "New connection from " + session.RemoteEndPoint);
            Buffers.Add(session, new List<byte>());

            SendToSession(session, Instruction.OpCode.INFO, new Dictionary<string, string> { { "message", "Welcome to JARVIS." } });
            SendToSession(session, Instruction.OpCode.AUTH);
        }

        void Server_OnException(Sender session, System.Exception e)
        {
            Shared.Log.Message("socket", "Exception from " + session.RemoteEndPoint + " of " + e.Message);
        }


        void Server_OnData(Sender session, byte[] data)
        {
            // We have to assume the buffer exists for the session
            Buffers[session].AddRange(data);

            int terminator = Buffers[session].IndexOf(JCP.TransmissionTerminator);
            while (terminator != -1)
            {

                Packet[] packets = Protocol.GetPackets(Buffers[session].GetRange(0, terminator).ToArray());
                foreach (Packet p in packets)
                {
                    foreach (Instruction i in p.GetInstructions())
                    {
                        Shared.Log.Message("socket", "Instruction Received -> " + i.Operation.ToString() + " from " + session.RemoteEndPoint.GetHost());

                        // Factory Pattern
                        ISocketCommand receivedCommand = CommandFactory.CreateCommand(i.Operation);

                        // Move forward?
                        if (receivedCommand.CanExecute())
                        {
                            receivedCommand.Execute(session, i.Parameters);
                        }
                    }
                }

                Buffers[session].RemoveRange(0, terminator + 1);

                // Look again
                terminator = Buffers[session].IndexOf(JCP.TransmissionTerminator);
            }
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


      



        public void SendToAllSessions(Instruction.OpCode type, Dictionary<string, string> arguments)
        {
            // Send to sessions
            foreach(Sender session in Server.Clients)
            {
                SendToSession(session, type, arguments);
            }
        }



        public void SendToSession(Sender session, Instruction.OpCode type)
        {
            SendToSession(session, type, new Dictionary<string, string> { });
        }

        public void SendToSession(Sender session, Instruction.OpCode type, Dictionary<string, string> arguments)
        {
            Shared.Log.Message("socket", "Sending " + type.ToString() + " to " + session.RemoteEndPoint);
            session.Send(Protocol.GetBytes(new Packet(type, arguments)));
        }
    }
}