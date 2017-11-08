using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Counter : ISocketCommand
    {
        public bool CanExecute()
        {
            return Program.HasCounterSupport;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            // Check Permission


            if ( parameters.ContainsKey("name") && parameters.ContainsKey("UPDATED_VALUE"))
            {
                Shared.Log.Message("counter", "Setting " + parameters["name"].Trim() + " => " + parameters["UPDATED_VALUE"].Trim());
                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["name"].Trim() + ".txt"), parameters["UPDATED_VALUE"].Trim());      
            }
        }
    }
}
