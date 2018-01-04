
using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;
namespace JARVIS.Shard.Services.Socket.Commands
{
    public class BinaryFile : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return Program.HasFileOutputs;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            // Check Permission


            if (parameters.ContainsKey("filename") && parameters.ContainsKey("content"))
            {
                Shared.Log.Message("file", "Setting " + parameters["filename"] + " => " + parameters["content"]);

                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["filename"].ToString()), parameters["content"].GetBytes());
            }
        }
    }
}
