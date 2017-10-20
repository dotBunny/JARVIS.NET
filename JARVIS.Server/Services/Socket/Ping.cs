using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Server.Services.Socket
{
    public class Ping : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            session.Send("PONG");
        }
    }
}