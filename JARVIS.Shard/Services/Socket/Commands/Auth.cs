using System.Collections.Generic;


namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Auth : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            // Send Auth
            Program.Client.Send(
                Shared.Services.Socket.Commands.Types.AUTH, 
                new Dictionary<string, string>() {
                    {"username",Program.Username},
                    {"password",Program.Password},
            });

            Shared.Log.Message("AUTH","Login Sent");
        }
    }
}