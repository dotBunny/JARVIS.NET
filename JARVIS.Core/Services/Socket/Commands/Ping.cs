using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class Ping : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            session.Send("PONG" + Shared.Net.SocketTerminator);
        }
    }
}