using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Foundation;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Wirecast : ISocketCommand
    {

        string appleScriptStreamProperty= "tell application \"Wirecast\"\n\r";

        public bool CanExecute()
        {
            return Settings.FeatureWirecastManipulation;
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {

            // Clean arguements
            string parsedArguments = "";
            Dictionary<string, string> layers = new Dictionary<string, string>();
            foreach (string s in parameters.Keys)
            {
                string lowerKey = s.ToLower();

                switch(s.ToLower())
                {
                    case "layer1":
                    case "0":
                        layers.Add("Master Layer 1", parameters[s].Trim());
                        parsedArguments += " L1:" + parameters[s].Trim();
                        break;
                    case "layer2":
                    case "1":
                        layers.Add("Master Layer 2", parameters[s].Trim());
                        parsedArguments += " L2:" + parameters[s].Trim();
                        break;
                    case "layer3":
                    case "2":
                        layers.Add("Master Layer 3", parameters[s].Trim());
                        parsedArguments += " L3:" + parameters[s].Trim();
                        break;
                    case "layer4":
                    case "3":
                        layers.Add("Master Layer 4", parameters[s].Trim());
                        parsedArguments += " L4:" + parameters[s].Trim();
                        break;
                    case "layer5":
                    case "4":
                        layers.Add("Master Layer 5", parameters[s].Trim());
                        parsedArguments += " L5:" + parameters[s].Trim();
                        break;
                }
               
            }



            // We do have something to do
            if ( layers.Count > 0 )
            {
                // Grab properties
                string appleScript = appleScriptStreamProperty;

                foreach (KeyValuePair<string, string> item in layers)
                {
                    appleScript += "\tset active shot of the layer named \"" + item.Key + "\" of last document to the shot named \"" + item.Value + "\" of last document\n\r";
                }

                appleScript += "\tgo last document\n\rend tell\n\r";

                NSAppleScript scriptObject = new NSAppleScript(appleScript);

                Shared.Log.Message("Wire", "Changing to " + parsedArguments.Trim());

                NSDictionary executeResponse = new NSDictionary();
                scriptObject.ExecuteAndReturnError(out executeResponse);
                if ( executeResponse != null && executeResponse.Keys != null && executeResponse.Keys.Length > 0 )
                {
                    Shared.Log.Error("Wire", "An error occured. " + executeResponse.Values[0].ToString());
                }

            }

        }
    }
}
