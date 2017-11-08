using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : ICommand
    {
        public void ExecuteCommand(Sender session, Shared.Protocol.Instruction instruction)
        {
            Server.Socket.SendToSession(session, 
                                        Shared.Protocol.Instruction.OpCode.PONG, 
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}