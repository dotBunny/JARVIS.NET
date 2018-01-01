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
            // Remove previous authentication
            if (Server.Socket.AuthenticatedUsers.ContainsKey(session))
            {
                Server.Socket.AuthenticatedUsers.Remove(session);
            }

            // Send auth request to session
            Server.Socket.SendToSession(session, Shared.Protocol.Instruction.OpCode.AUTH);
        }
    }
}