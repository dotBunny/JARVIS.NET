using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Wirecast : ISocketCommand
    {
        public bool CanExecute()
        {
            return Program.HasWirecastSupport;
        }
        public void Execute(Dictionary<string, string> parameters) 
        {
            string parsedArguments = "";
            foreach(string s in parameters.Keys)
            {
                parsedArguments += parameters[s] + " ";
            }
            parsedArguments.Trim();

            // TODO: Handle other platforms, but for now, just macOS
            Shared.Log.Message("Wire", "Changing to " + parsedArguments);
            Shared.Platform.Run(System.IO.Path.Combine(Shared.Platform.GetBaseDirectory(), "Resources", "macOS", "Wirecast.appleScript"), parsedArguments, true);
        }
    }
}
