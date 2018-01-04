using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class AuthDiscord : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Server.Services.GetService<SocketService>().AuthenticatedUsers[session].HasPemission(Discord.DiscordService.ScopeAuthentication);
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Server.Services.GetService<Discord.DiscordService>().Stop();
            Server.Services.GetService<Discord.DiscordService>().Start();
        }
    }
}