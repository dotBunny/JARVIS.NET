using System;
using JARVIS.Shared.Services.Socket;

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

        public UsersRow()
        {
            
        }
        public UsersRow(int rowID, string username, bool shard)
        {
            ID = rowID;
            Username = username;
            CanShard = shard;
            LastLogin = DateTime.Now;
        }
        public UsersRow(int rowID, string username, bool shard, string datestamp)
        {
            ID = rowID;
            Username = username;
            CanShard = shard;
            LastLogin = DateTime.Parse(datestamp);
        }
        public UsersRow(int rowID, string username, bool shard, DateTime datestamp)
        {
            ID = rowID;
            Username = username;
            CanShard = shard;
            LastLogin = datestamp;
        }
    }
}