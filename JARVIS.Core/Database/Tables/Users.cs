using System;

namespace JARVIS.Core.Database.Tables
{
    public static class Users
    {
        public class UsersObject
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

        public static string CreateSQL()
        {
            return "CREATE TABLE IF NOT EXISTS \"Users\" (" +
                "\"ID\" integer PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                "\"Username\" varchar(128) NOT NULL, " +
                "\"Password\" varchar(128), " +
                "\"CanShard\" integer(1) NOT NULL DEFAULT(0), " +
                "\"LastLogin\" varchar(128));";
        }

        public static UsersObject GetUserObject(int id)
        {
            return new UsersObject();
        }

        public static UsersObject GetUserObject(string username)
        {
            return new UsersObject();
        }

        public static UsersObject Login(string username, string password)
        {



           // DateTime.Now.tostring
            return new UsersObject();



        }

        //public static void Set(string name, int newValue)
        //{
        //    name = Shared.Strings.Truncate(name, 128);

        //    Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);

        //    Server.Database.ExecuteNonQuery(
        //        "REPLACE INTO \"Counters\" (\"Name\", \"Value\") VALUES (\"" + name + "\", " + newValue + ")"
        //    );
        //}
    }
}