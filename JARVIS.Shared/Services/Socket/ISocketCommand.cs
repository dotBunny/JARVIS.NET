using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;

namespace JARVIS.Shared.Services.Socket
{
    public interface ISocketCommand
    {
        bool CanExecute(Sender session);
        void Execute(Sender session, Dictionary<string, InstructionParameter> parameters);
    }
}
