using System.Collections.Generic;
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
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Client.Send(Shared.Protocol.Instruction.OpCode.PONG,
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}