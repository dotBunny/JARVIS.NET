using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Auth : ISocketCommand
    {
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            // Send Auth
            Program.Client.Send(
                Instruction.OpCode.LOGIN, 
                new Dictionary<string, string> {
                    {"username",Program.Username},
                    {"password",Program.Password}
            });

            Shared.Log.Message("AUTH","Login Sent");
        }
    }
}