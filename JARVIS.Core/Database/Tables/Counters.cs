namespace JARVIS.Core.Database.Tables
{
    public static class Counters
    {
        public static string CreateSQL()
        {
            return "CREATE TABLE IF NOT EXISTS \"Counters\" (" +
                "\"Name\" varchar(128) PRIMARY KEY NOT NULL, " +
                "\"Value\" integer(128) NOT NULL DEFAULT(0));";
        }

        public static int Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            int returnValue = 0;
            Provider.ProviderResult result = Server.Database.ExecuteQuery("SELECT \"Value\" FROM \"Counters\" WHERE \"Name\" = \"" + name + "\" LIMIT 1");

            if ( result.Data != null && result.Data.HasRows ) 
            {
                result.Data.Read();
                int.TryParse(result.Data.GetString(0), out returnValue);
            }

            return returnValue;
        }

        public static void Set(string name, int newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO \"Counters\" (\"Name\", \"Value\") VALUES (\"" + name + "\", " + newValue + ")"
            );
        }
    }
}
