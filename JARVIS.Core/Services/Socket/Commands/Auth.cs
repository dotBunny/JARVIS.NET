using JARVIS.Shared.Services.Socket;
namespace JARVIS.Core.Services.Socket.Commands
{
    public class Auth : ICommand
    {
        public void ExecuteCommand(Sender session, Shared.Protocol.Instruction instruction)
        {
            
            Shared.Log.Message("Login", "new login from ");
            //ession.Send("PONG" + Shared.Net.SocketTerminator);
        }
    }
}