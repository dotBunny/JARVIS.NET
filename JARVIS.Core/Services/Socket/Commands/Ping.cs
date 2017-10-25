using System.Collections.Generic;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            SocketService.SendToSession(session, 
                                        Shared.Services.Socket.Commands.Types.PONG, 
                                        string.Empty,
                                        new Dictionary<string, string>() { { "message", "Hi!" } });
        }
    }
}