using SQLite.Net.Attributes;

namespace JARVIS.Server.Tables
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
    }
}
