using System.Collections.Generic;
using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Mac.Services.Socket.Commands
{
    public class Auth : ISocketCommand
    {
        ISocketClient Client;

        public Auth(ISocketClient client)
        {
            Client = client;
        }
        public bool CanExecute(Sender session)
        {
            return true;
        }
        public void Execute(Sender session, Dictionary<string, InstructionParameter> parameters)
        {
            // Send Auth
            Client.Send(
                Instruction.OpCode.LOGIN,
                new Dictionary<string, string> {
                {"username",Settings.SessionUsername},
                {"password",Settings.SessionPassword}
            });

            Shared.Log.Message("AUTH", "Login Sent");
        }
    }
}