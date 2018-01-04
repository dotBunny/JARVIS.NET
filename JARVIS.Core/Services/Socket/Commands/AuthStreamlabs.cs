using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class AuthStreamlabs : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Server.Services.GetService<SocketService>().AuthenticatedUsers[session].HasPemission(Streamlabs.StreamlabsService.ScopeAuthentication);
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Server.Services.GetService<Streamlabs.StreamlabsService>().Stop();
            Server.Services.GetService<Streamlabs.StreamlabsService>().Start();
        }
    }
}