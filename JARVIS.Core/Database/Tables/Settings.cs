using SQLite.Net.Attributes;

namespace JARVIS.Core.Database.Tables
{
    public class Settings : ITable
    {

        public const string ServerHostID = "Server.Host";
        public const string ServerWebPortID = "Server.WebPort";
        public const string ServerSocketPortID = "Server.SocketPort";

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
            Shared.Log.Message("DB", "Set " + key + ":" + newValue);
            Server.Database.Connection.InsertOrReplace(new Settings()
            {
                Name = key,
                Value = newValue
            });
        }


        // TODO: Add Get
    }
}
