using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Ping : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            Program.Client.Send(Instruction.OpCode.PONG, new Dictionary<string, string> { { "message", "Hi!" } });
        }
    }
}