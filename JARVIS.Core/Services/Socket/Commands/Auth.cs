using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
namespace JARVIS.Core.Services.Socket.Commands
{
    public class Auth : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string,string> parameters)
        {
            
            Shared.Log.Message("Login", "new login from ");
            //ession.Send("PONG" + Shared.Net.SocketTerminator);
        }
    }
}