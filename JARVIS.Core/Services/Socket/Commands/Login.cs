using System.Collections.Generic;
using JARVIS.Shared.Services.Socket;
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

            if (parameters.ContainsKey("username") && parameters.ContainsKey("password"))
            {

                Database.Rows.UsersRow user = Database.Tables.Users.Login(parameters["username"], parameters["password"]);

                if (user.ID != -1 && user.CanShard)
                {
                    Shared.Log.Message("Login", "Login from " + user.Username + " successful.");

                    SocketUser newUser = new SocketUser(session)
                    {
                        DataObject = user
                    };

                    // Add to authenticated list
                    if (Server.Socket.AuthenticatedUsers.ContainsKey(session))
                    {
                        Server.Socket.AuthenticatedUsers[session] = newUser;
                    }
                    else
                    {
                        Server.Socket.AuthenticatedUsers.Add(session, newUser);
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
                Server.Socket.SendToSession(session, Shared.Protocol.Instruction.OpCode.LOGIN_FAIL);
            }
        }
    }
}