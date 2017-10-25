using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Pong : JARVIS.Shard.Services.Socket.ICommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            Shared.Log.Message("socket", "Keep Alive");
        }
    }
}
