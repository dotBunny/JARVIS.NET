namespace JARVIS.Core.Database.Tables
{
    public class Counters : ITable
    {
        public string Name { get; private set; }
        public int Value { get; private set;  }

        public Counters(string name, int newValue)
        {
            Name = name;
            Value = newValue;
        }

        public static string GetTableName()
        {
            return "Counters";
        }

        public static string GetCreation()
        {
            return "CREATE TABLE \"" + GetTableName() + "\" (\"Name\" varchar(128) NOT NULL, \"Value\" int PRIMARY KEY (\"Name\"))";
        }


        public static void Set(string name, int newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO \"" + GetTableName() + "\" (\"Name\", \"Value\") VALUES (\"" + name + "\", " + newValue +")"
            );

        }

        public static int Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery("SELECT * FROM \"" + GetTableName() + "\" WHERE \"Name\" = \"" + name + "\" LIMIT 1");
            if ( result.Data != null && result.Data.HasRows ) 
            {
                Counters row = result.Data.Single(
                    r => new Counters(
                        (string)r["Name"], 
                        int.Parse(r["Value"].ToString())));

                return row.Value;
            }
            return 0;
        }
    }
}
