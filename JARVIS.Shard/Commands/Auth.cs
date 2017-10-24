using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Commands
{
    public static class Auth
    {
        public static void Command()
        {
            // Send Auth
            Program.Server.Send(Shared.Net.GetSocketData("AUTH", new Dictionary<string, string>() {
                    {"username",Program.Username},
                    {"password",Program.Password},
                }));


            Shared.Log.Message("auth","Login Sent");
        }
    }
}