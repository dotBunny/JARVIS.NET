using System.Collections.Generic;
using JARVIS.Client;
using JARVIS.Client.Mac;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Counter : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return Settings.FeatureCounterOutputs;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // Check Permission


            if ( parameters.ContainsKey("name") && parameters.ContainsKey("UPDATED_VALUE"))
            {
                Shared.Log.Message("counter", "Setting " + parameters["name"].Trim() + " => " + parameters["UPDATED_VALUE"].Trim());
                Shared.IO.WriteContents(System.IO.Path.Combine(Settings.FeatureCounterOutputsPath, parameters["name"].Trim() + ".txt"), parameters["UPDATED_VALUE"].Trim());      
            }
        }
    }
}
