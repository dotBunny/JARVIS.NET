using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket.Commands
{
    public class SpotifyReauth : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Server.Spotify.Stop();
            Server.Spotify.Start();
        }
    }
}