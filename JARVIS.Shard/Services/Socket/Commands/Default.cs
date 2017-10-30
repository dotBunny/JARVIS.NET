using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Default : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
        }
    }
}
