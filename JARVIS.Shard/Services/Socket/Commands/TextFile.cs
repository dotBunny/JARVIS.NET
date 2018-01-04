using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class TextFile : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Program.HasFileOutputs;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // Check Permission


            if ( parameters.ContainsKey("name") && parameters.ContainsKey("content"))
            {
                Shared.Log.Message("file", "Setting " + parameters["name"].Trim() + " => " + parameters["content"].Trim());
                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["filename"].Trim()), parameters["content"].Trim());      
            }
        }
    }
}
