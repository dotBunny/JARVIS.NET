using System.Collections.Generic;
using JARVIS.Core.Database.Rows;
using JARVIS.Shared.Services.Socket;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core.Services.Socket
{
    public class SocketUser
    {
        
        public Sender Session;
        List<string> UserPermissions;

        public SocketUser(Sender session, UsersRow data)
        {
            Session = session;
            UserPermissions = data.Scope;
        }
        public void Terminate()
        {
            Server.Services.GetService<SocketService>().AuthenticatedUsers.Remove(Session);
        }

        public bool HasPemission(string scope)
        {
            if (UserPermissions.Contains(scope))
            {
                return true;
            }
            return false;
        }
    }
}
