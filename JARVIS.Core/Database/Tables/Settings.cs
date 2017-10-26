using System.Collections.Generic;
using SQLite;

namespace JARVIS.Core.Database.Tables
{
    public class Settings : ITable
    {

        public const string ServerHostID = "Server.Host";
        public const string ServerWebPortID = "Server.WebPort";
        public const string ServerSocketPortID = "Server.SocketPort";
        public const string ServerSocketEncryptionKeyID = "Server.SocketEncryptionKey";

        public static string GetTableName()
        {
            return "Settings";
        }

        [PrimaryKey, MaxLength(128), NotNull]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Value { get; set; }

        public static void Set(string key, string newValue)
        {
            Shared.Log.Message("DB", "Set " + key + " to " + newValue);
            Server.Database.Connection.InsertOrReplaceAsync(new Settings()
            {
                Name = key,
                Value = newValue
            }).Wait();
        }

        public static Dictionary<string, string> GetAll()
        {
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
            var result = Server.Database.Connection.QueryAsync<Settings>("SELECT * FROM \"" + GetTableName() + "\"").GetAwaiter();

            // Add to raw settings dictionary
            foreach (Settings setting in result.GetResult())
            {
                returnDictionary.Add(setting.Name, setting.Value);
            }
            return returnDictionary;
        }


        // TODO: Add Get
    }
}
