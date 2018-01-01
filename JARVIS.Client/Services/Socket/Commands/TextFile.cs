using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Services.Socket.Commands
{
    public class TextFile : JARVIS.Shared.Services.Socket.ISocketCommand
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
                Shared.Log.Message("file", "Setting " + parameters["filename"].Trim() + " => " + parameters["content"].Trim());
                Shared.IO.WriteContents(System.IO.Path.Combine(Settings.FeatureFileOutputsPath, parameters["filename"].Trim()), parameters["content"].Trim());
            }
        }
    }
}
