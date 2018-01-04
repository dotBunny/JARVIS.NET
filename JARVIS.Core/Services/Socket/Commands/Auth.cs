using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;
namespace JARVIS.Core.Services.Socket.Commands
{
    public class Auth : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Server.Services.GetService<SocketService>().AuthenticatedUsers[session].HasPemission(Streamlabs.StreamlabsService.ScopeAuthentication);
        }

        public void Execute(Sender session, Dictionary<string,string> parameters)
        {
            // Get socket service
            SocketService socket = Server.Services.GetService<SocketService>();

            // Remove previous authentication
            if (socket.AuthenticatedUsers.ContainsKey(session))
            {
                socket.AuthenticatedUsers.Remove(session);
            }

            // Send auth request to session
            socket.SendToSession(session, Shared.Protocol.Instruction.OpCode.AUTH);
        }
    }
}