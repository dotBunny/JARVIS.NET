using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class AUTH : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            
            Shared.Log.Message("Login", "new login from ");
            //ession.Send("PONG" + Shared.Net.SocketTerminator);
        }
    }
}