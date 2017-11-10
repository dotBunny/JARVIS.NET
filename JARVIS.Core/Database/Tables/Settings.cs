using System;
using System.Collections.Generic;

namespace JARVIS.Core.Database.Tables
{
    public static class Settings
    {
        public class SettingsObject
        {
            public string Name;
            public string Value;
        }

        public const string DatabaseVersionID = "Database.Version";
        public const string ServerHostID = "Server.Host";
        public const string ServerSocketEncryptionID = "Server.SocketEncryption";
        public const string ServerSocketEncryptionKeyID = "Server.SocketEncryptionKey";
        public const string ServerSocketPortID = "Server.SocketPort";
        public const string ServerWebPortID = "Server.WebPort";

        public static string CreateSQL()
        {
            return "CREATE TABLE IF NOT EXISTS \"Settings\" (" +
                "\"Name\" varchar(128) PRIMARY KEY NOT NULL, " +
                "\"Value\" varchar(128));";
        }

        public static string Get(string key)
        {
            Provider.ProviderResult result = Server.Database.ExecuteSingleQuery(
                "SELECT \"Value\" FROM \"Settings\" WHERE Name = \"" + key + "\" LIMIT 1");

            if (result.Data != null && result.Data.HasRows)
            {
                result.Data.Read();
                return result.Data.GetString(0);
            }

            return string.Empty;
        }

        public static Dictionary<string, string> GetAll()
        {
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT \"Name\",\"Value\" FROM \"Settings\""
            );

            if (result.Data != null && result.Data.HasRows)
            {
                while (result.Data.Read())
                {
                    returnDictionary.Add(
                        result.Data.GetString(0), 
                        result.Data.GetString(1));
                    result.Data.NextResult();
                }
            }

            return returnDictionary;
        }

        public static void Set(string key, string newValue)
        {
            key = Shared.Strings.Truncate(key, 128);
            newValue = Shared.Strings.Truncate(newValue, 128);

            Shared.Log.Message("DB", "Set " + key + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO \"Settings\" (\"Name\", \"Value\") VALUES (\"" + key + "\", \"" + newValue + "\")"
            );
        }
    }
}
