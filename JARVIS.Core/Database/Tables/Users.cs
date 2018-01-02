using System;
using System.Collections.Generic;
using JARVIS.Core.Database.Rows;
using JARVIS.Shared;


namespace JARVIS.Core.Database.Tables
{
    public static class Users
    {
        public static UsersRow Login(string username, string password)
        {
            UsersRow returnUser = new UsersRow();

            // Hash Password
            password = (password + Server.Config.Salt).SHA512();

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT ID, Username, Scope, LastLogin FROM Users WHERE Username = @Username AND Password = @Password LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Username",username},
                    {"@Password",password}
            }, System.Data.CommandBehavior.SingleResult);

            if (result.Data != null && result.Data.HasRows)
            {
                result.Data.Read();

                // Apply Data
                returnUser.ID = result.Data.GetInt32(0);
                returnUser.Username = result.Data.GetString(1);
                returnUser.Scope.AddRange(result.Data.GetString(2).Split(' '));

                // Create if the last login is null
                if (!result.Data.IsDBNull(result.Data.GetOrdinal("LastLogin")))
                {
                    returnUser.LastLogin = DateTime.Parse(result.Data.GetString(3));
                }

                // Update last login time
                Server.Database.ExecuteNonQuery(
                  "UPDATE Users SET LastLogin = @LastLogin WHERE Username = @Username",
                  new Dictionary<string, object>() {
                    {"@LastLogin",DateTime.Now.ToLongDateString()},
                    {"@Username",username},
                  }
                );
            }

            return returnUser;
        }
    }
}