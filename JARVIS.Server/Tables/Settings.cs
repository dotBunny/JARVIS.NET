using SQLite.Net.Attributes;

namespace JARVIS.Server.Tables
{
    public class Settings : ITable
    {
        public static string GetTableName()
        {
            return "Settings";
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(128), NotNull]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Value { get; set; }
    }
}
