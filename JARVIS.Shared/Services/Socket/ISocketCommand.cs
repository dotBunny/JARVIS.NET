using System;
using System.Collections.Generic;

namespace JARVIS.Shared.Services.Socket
{
    public interface ISocketCommand
    {
        bool CanExecute(Sender session);
        void Execute(Sender session, Dictionary<string, string> parameters);
    }
}
