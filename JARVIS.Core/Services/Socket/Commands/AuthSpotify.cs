using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class AuthSpotify : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // TODO: This should only send back to the sender maybe??? - and only if authenticated ?

            Server.Services.GetService<Spotify.SpotifyService>().Stop();
            Server.Services.GetService<Spotify.SpotifyService>().Start();
        }
    }
}