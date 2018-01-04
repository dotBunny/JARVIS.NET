using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shared.Services.Socket.Commands
{
    public class Default : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
        }
    }
}
