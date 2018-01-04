using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            
            Server.Services.GetService<SocketService>().SendToSession(session,
                                        Instruction.OpCode.PONG,
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}