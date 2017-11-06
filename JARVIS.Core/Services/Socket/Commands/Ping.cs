using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : ICommand
    {
        public void ExecuteCommand(Sender session, Protocol.Packet packet)
        {
            Server.Socket.SendToSession(session, 
                                        Shared.Services.Socket.Commands.Types.PONG, 
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}