using System;
namespace JARVIS.Core.Database.Rows
{
    public class UsersRow
    {
        public int ID = -1;
        public string Username = "Undefined";
        public bool CanShard = false;
        public DateTime LastLogin;

        public bool IsValid()
        {
            return (ID != -1);
        }
    }
}
