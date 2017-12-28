using System.Collections.Generic;
using JARVIS.Client.Mac;
                
namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Auth : JARVIS.Shared.Services.Socket.ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, string> parameters)
        {
            // Send Auth
            MainClass.Client.Send(
                Shared.Protocol.Instruction.OpCode.AUTH, 
                new Dictionary<string, string>() {
                    {"username",Program.Username},
                    {"password",Program.Password},
            });

            Shared.Log.Message("AUTH","Login Sent");
        }
    }
}