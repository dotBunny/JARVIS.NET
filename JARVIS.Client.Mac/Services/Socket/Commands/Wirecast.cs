using System.Collections.Generic;
using JARVIS.Client;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Wirecast : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return Settings.FeatureWirecastManipulation;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters) 
        {
            string parsedArguments = "";
            foreach(string s in parameters.Keys)
            {
                parsedArguments += string.Format("\"{0}\" ", parameters[s]);
            }
            parsedArguments.Trim();

            // TODO: Handle other platforms, but for now, just macOS
            Shared.Log.Message("Wire", "Changing to " + parsedArguments);
            Shared.Platform.Run(System.IO.Path.Combine(Shared.Platform.GetBaseDirectory(), "Resources", "Wirecast.appleScript"), parsedArguments, true);
        }
    }
}
