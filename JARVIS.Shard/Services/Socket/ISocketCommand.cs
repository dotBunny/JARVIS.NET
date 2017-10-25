using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket
{
    public interface ISocketCommand
    {
        bool CanExecute();
        void Execute(Dictionary<string, string> parameters);
    }
}
