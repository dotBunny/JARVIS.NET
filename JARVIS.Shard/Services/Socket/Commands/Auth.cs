using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Services.Socket.Commands
{
    public class Auth : JARVIS.Shard.Services.Socket.ICommand
    {
        public bool CanExecute()
        {
            return true;
        }
        public void Execute(Dictionary<string, string> parameters)
        {
            // Send Auth
            Program.Server.Send(
                Shared.Services.Socket.Commands.Types.AUTH, 
                string.Empty, 
                new Dictionary<string, string>() {
                    {"username",Program.Username},
                    {"password",Program.Password},
            });

            Shared.Log.Message("AUTH","Login Sent");
        }
    }
}