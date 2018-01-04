using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class AuthSpotify : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Server.Services.GetService<SocketService>().AuthenticatedUsers[session].HasPemission(Spotify.SpotifyService.ScopeAuthentication);
        }

        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            Server.Services.GetService<Spotify.SpotifyService>().Stop();
            Server.Services.GetService<Spotify.SpotifyService>().Start();
        }
    }
}