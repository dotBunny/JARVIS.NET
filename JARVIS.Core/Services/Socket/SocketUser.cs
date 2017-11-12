using System;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket
{
    public class SocketUser
    {
        public Database.Rows.UsersRow DataObject;
        public Sender Session;

        public SocketUser(Sender session)
        {
            Session = session;
        }
        public void Terminate()
        {
            Server.Socket.AuthenticatedUsers.Remove(Session);
        }
    }
}
