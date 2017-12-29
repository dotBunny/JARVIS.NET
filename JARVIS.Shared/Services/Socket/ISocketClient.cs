using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;

namespace JARVIS.Shared.Services.Socket
{
    public interface ISocketClient
    {
        void Start();
        void Stop();

        void Send(Instruction.OpCode type, Dictionary<string, string> parameters);
        void Send(Packet packet);
        void Send(Packet[] packets);

    }
}
