using System;
using JARVIS.Core.Database.Rows;


namespace JARVIS.Core.Database.Tables
{
    public static class Users
    {



        public static UsersRow GetUserObject(int id)
        {
            return new UsersRow();
        }

        public static UsersRow GetUserObject(string username)
        {
            return new UsersRow();
        }

        public static UsersRow Login(string username, string password)
        {


           // Provider.ProviderResult result = Server.Database.ExecuteSingleQuery(
           //    "SELECT \"ID\",\"Username\",\"CanShard\",\"LastLogin\" " +
           //     "FROM \"Users\" " +
           //     "WHERE Username = \"" + username + "\" AND Password = \"" + password + "\" " +
           //     "LIMIT 1");

           // if (result.Data != null && result.Data.HasRows)
           // {
           //     result.Data.Read();
           //     return result.Data.GetString(0);
           // }

           //// DateTime.Now.tostring



            return new UsersRow();
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