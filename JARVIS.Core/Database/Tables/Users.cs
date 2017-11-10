namespace JARVIS.Core.Database.Tables
{
    public static class Users
    {
        public class UsersObject
        {
            public int ID;
            public string Username;
            public bool CanShard;
            public string LastLogin;
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

        public static bool IsValidLogin(string username, string password)
        {
            return false;
        }

        public static void Set(string name, int newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO \"Counters\" (\"Name\", \"Value\") VALUES (\"" + name + "\", " + newValue + ")"
            );
        }

        public static void UpdateLastLoginDate(int id)
        {
            
        }

        public static void UpdateLastLoginDate(string username)
        {
            
        }
    }
}