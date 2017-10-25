using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket
{
    public interface ICommand
    {
        bool CanExecute();
        void Execute(Dictionary<string, string> parameters);
    }
}
