using System.Collections.Generic;
using JARVIS.Shared.Protocol;

namespace JARVIS.Shared.Services.Socket.Commands
{
    public class Info : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            foreach(string s in parameters.Keys) 
            {
                Log.Message("info", s + " => " + parameters[s]);    
            }
        }
    }
}
