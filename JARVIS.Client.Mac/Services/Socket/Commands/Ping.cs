using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Ping : ISocketCommand
    {
        ISocketClient Client;

        public Ping(ISocketClient client)
        {
            Client = client;
        }
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            Client.Send(Instruction.OpCode.PONG, new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}