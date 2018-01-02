using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            
            Server.Provider.GetService<SocketService>().SendToSession(session, 
                                        Shared.Protocol.Instruction.OpCode.PONG,
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}