using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Default : JARVIS.Shard.Services.Socket.ICommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            //foreach (string s in parameters.Keys)
            //{
            //    Shared.Log.Message("info", s + " => " + parameters[s]);
            //}
        }
    }
}
