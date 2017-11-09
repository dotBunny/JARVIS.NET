using System;
using System.Collections.Generic;

namespace JARVIS.Shared.Services.Socket
{
    public interface ISocketCommand
    {
        bool CanExecute();
        void Execute(Sender session, Dictionary<string, string> parameters);
    }
}
