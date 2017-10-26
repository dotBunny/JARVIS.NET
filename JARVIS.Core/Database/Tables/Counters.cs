using SQLite;

namespace JARVIS.Core.Database.Tables
{
    public class Counters : ITable
    {
        public static string GetTableName()
        {
            return "Counters";
        }

        [PrimaryKey, MaxLength(128), NotNull]
        public string Name { get; set; }
        [MaxLength(128)]
        public int Value { get; set; }

        // TODO: Add Set/Get

        public static void Set(string name, int newValue)
        {
            Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);
            Server.Database.Connection.InsertOrReplaceAsync(new Counters()
            {
                Name = name,
                Value = newValue
            });
        }

        public static int Get(string name)
        {
            var result = Server.Database.Connection.QueryAsync<Counters>("SELECT * FROM \"" + GetTableName() + "\" WHERE \"Name\" = \"" + name + "\" LIMIT 1").GetAwaiter();
            if (result.GetResult().Count > 0) {

                return result.GetResult()[0].Value;
            }
            return 0;
        }
    }
}
