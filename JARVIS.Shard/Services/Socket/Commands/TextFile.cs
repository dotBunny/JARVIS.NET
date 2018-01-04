using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class TextFile : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Program.HasFileOutputs;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            // Check Permission


            if ( parameters.ContainsKey("name") && parameters.ContainsKey("content"))
            {
                Shared.Log.Message("file", "Setting " + parameters["name"] + " => " + parameters["content"]);
                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["filename"].ToString()), parameters["content"].ToString());      
            }
        }
    }
}
