using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Counter : JARVIS.Shard.Services.Socket.ICommand
    {
        public bool CanExecute()
        {
            return Program.HasOutputSupport;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            // Check Permission


            if ( parameters.ContainsKey("name") && parameters.ContainsKey("UPDATED_VALUE"))
            {
                Shared.Log.Message("counter", "Setting " + parameters["name"] + " => " + parameters["UPDATED_VALUE"]);
                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["name"] + ".txt"), parameters["UPDATED_VALUE"]);      
            }
        }
    }
}
