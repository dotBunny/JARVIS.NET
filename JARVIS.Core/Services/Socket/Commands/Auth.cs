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

            if ( parameters.ContainsKey("username") && parameters.ContainsKey("password") ) {


                Shared.Log.Message("Login", "new login from ");
            } else {
                Shared.Log.Error("Login", "Invalid Login Attempt from " + session.RemoteEndPoint.GetHost());
            }
        }
    }
}