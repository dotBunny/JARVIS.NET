using System;
using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Services.Socket.Commands
{
    /// <summary>
    /// Write To Binary File Command
    /// </summary>
    public class BinaryFile : ISocketCommand
    {
        /// <summary>
        /// Can this command be executed?
        /// </summary>
        /// <returns><c>true</c>, if settings look good, <c>false</c> otherwise.</returns>
        public bool CanExecute(Sender session)
        {
            return Settings.FeatureFileOutputs;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="session">The user session.</param>
        /// <param name="parameters">The parameters (filename, content) to use while executing the command.</param>
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            // Check Permission
            if (parameters.ContainsKey("filename") && parameters.ContainsKey("content"))
            {
                Shared.Log.Message("file", "Setting " + parameters["filename"] + " => " + parameters["content"]);

                Shared.IO.WriteContents(System.IO.Path.Combine(Settings.FeatureFileOutputsPath, parameters["filename"].ToString()), parameters["content"].GetBytes());
            }
        }
    }
}
