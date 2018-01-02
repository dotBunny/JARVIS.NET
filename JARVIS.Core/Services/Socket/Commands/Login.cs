using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;
namespace JARVIS.Core.Services.Socket.Commands
{
    public class Login : ISocketCommand
    {
        public bool CanExecute()
        {
            return true;
        }

        public void Execute(Sender session, Dictionary<string, string> parameters)
        {

            // Get socket service
            SocketService socket = Server.Services.GetService<SocketService>();

            if (parameters.ContainsKey("username") && parameters.ContainsKey("password"))
            {

                Database.Rows.UsersRow user = Database.Tables.Users.Login(parameters["username"], parameters["password"]);

                if (user.ID != -1)
                {
                    Shared.Log.Message("Login", "Login from " + user.Username + " successful.");

                    SocketUser newUser = new SocketUser(session, user);

                    // Add to authenticated list
                    if (socket.AuthenticatedUsers.ContainsKey(session))
                    {
                        socket.AuthenticatedUsers[session] = newUser;
                    }
                    else
                    {
                        socket.AuthenticatedUsers.Add(session, newUser);
                    }
                }
                else
                {
                    Shared.Log.Error("Login", "Invalid Login Attempt (E0) from " + session.RemoteEndPoint.GetHost());
                }
            }
            else
            {
                Shared.Log.Error("Login", "Invalid Login Attempt (E1) from " + session.RemoteEndPoint.GetHost());
                socket.SendToSession(session, Shared.Protocol.Instruction.OpCode.LOGIN_FAIL);
            }
        }
    }
}