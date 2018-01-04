using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Foundation;
using JARVIS.Shared.Protocol;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class WirecastLayers : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Settings.FeatureWirecastManipulation;
        }

        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
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
                    case "1":
                    case "l1":
                        layers.Add("Master Layer 1", parameters[s].ToString());
                        parsedArguments += " L1:" + parameters[s];
                        break;
                    case "layer2":
                    case "2":
                    case "l2":
                        layers.Add("Master Layer 2", parameters[s].ToString());
                        parsedArguments += " L2:" + parameters[s];
                        break;
                    case "layer3":
                    case "3":
                    case "l3":
                        layers.Add("Master Layer 3", parameters[s].ToString());
                        parsedArguments += " L3:" + parameters[s];
                        break;
                    case "layer4":
                    case "4":
                    case "l4":
                        layers.Add("Master Layer 4", parameters[s].ToString());
                        parsedArguments += " L4:" + parameters[s];
                        break;
                    case "layer5":
                    case "5":
                    case "l5":
                        layers.Add("Master Layer 5", parameters[s].ToString());
                        parsedArguments += " L5:" + parameters[s];
                        break;
                }
               
            }

            // We do have something to do
            if ( layers.Count > 0 )
            {
                // Create apple script command
                string appleScript = "tell application \"Wirecast\"\n\r";

                // Add layers to change
                foreach (KeyValuePair<string, string> item in layers)
                {
                    appleScript += "\tset active shot of the layer named \"" + item.Key + "\" of last document to the shot named \"" + item.Value + "\" of last document\n\r";
                }

                // Append finalizing
                appleScript += "\tgo last document\n\rend tell\n\r";

                NSAppleScript scriptObject = new NSAppleScript(appleScript);

                Shared.Log.Message("Wirecast", "Changing to " + parsedArguments.Trim());

                NSDictionary executeResponse = new NSDictionary();
                scriptObject.ExecuteAndReturnError(out executeResponse);
                if ( executeResponse != null && executeResponse.Keys != null && executeResponse.Keys.Length > 0 )
                {
                    Shared.Log.Error("Wirecast", "An error occured. " + executeResponse.Values[0]);
                }

            }

        }
    }
}
