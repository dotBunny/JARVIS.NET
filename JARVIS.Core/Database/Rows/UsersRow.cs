using System;
using System.Collections.Generic;

namespace JARVIS.Core.Database.Rows
{
    public class UsersRow
    {
        public int ID = -1;
        public string Username = "Undefined";
        public List<string> Scope = new List<string>();
        public DateTime LastLogin;

        public bool IsValid()
        {
            return (ID != -1);
        }

        public UsersRow()
        {
            
        }
        public UsersRow(int rowID, string username, string scopes)
        {
            ID = rowID;
            Username = username;
            if (!string.IsNullOrEmpty(scopes) || scopes != "")
            {
                Scope.AddRange(scopes.Split(' '));
            }
            LastLogin = DateTime.Now;
        }
        public UsersRow(int rowID, string username, string scopes, string datestamp)
        {
            ID = rowID;
            Username = username;
            if (!string.IsNullOrEmpty(scopes))
            {
                Scope.AddRange(scopes.Split(' '));
            }
            LastLogin = DateTime.Parse(datestamp);
        }
        public UsersRow(int rowID, string username, string scopes, DateTime datestamp)
        {
            ID = rowID;
            Username = username;
            if (!string.IsNullOrEmpty(scopes) || scopes != "")
            {
                Scope.AddRange(scopes.Split(' '));
            }
            LastLogin = datestamp;
        }
    }
}