﻿using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Fail : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            Shared.Log.Message("AUTH", "Failed");
        }
    }
}