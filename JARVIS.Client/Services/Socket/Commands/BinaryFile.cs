using System;
using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Services.Socket.Commands
{
    public class BinaryFile : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return Settings.FeatureFileOutputs;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // Check Permission
            if (parameters.ContainsKey("filename") && parameters.ContainsKey("content"))
            {
                // COnvert our file content to a byte array. This isn't the most efficient thing, it increases the amount of data being transfered around.
                // TODO: Maybe investigate just encoding the data to a string??? and then decode it...
                byte[] fileContent = Convert.FromBase64String(parameters["content"]);
                                     
                Shared.Log.Message("file", "Setting " + parameters["filename"].Trim() + " => " + fileContent.Length);

                Shared.IO.WriteContents(System.IO.Path.Combine(Settings.FeatureFileOutputsPath, parameters["filename"].Trim()), fileContent);
            }
        }
    }
}
