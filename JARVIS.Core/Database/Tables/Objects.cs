using System.Collections.Generic;
using JARVIS.Core.Database.Rows;

namespace JARVIS.Core.Database.Tables
{
    /// <summary>
    /// JARVIS Objects Table
    /// </summary>
    public static class Objects
    {
       
        /// <summary>
        /// Get the specified row from the Objects table.
        /// </summary>
        /// <returns>The specified row.</returns>
        /// <param name="key">The settings key.</param>
        public static ObjectsRow Get(string key)
        {
            key = Shared.Strings.Truncate(key, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Value FROM Objects WHERE Name = @Name LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Name",key}
            }, System.Data.CommandBehavior.SingleResult);

            if (result.Data != null && result.Data.HasRows)
            {
                result.Data.Read();
                return new ObjectsRow(key, result.Data.GetString(0));
            }

            return new ObjectsRow();
        }

        /// <summary>
        /// Get all of the rows from the Objects table.
        /// </summary>
        /// <returns>All rows from the table.</returns>
        public static List<ObjectsRow> GetAll()
        {
            List<ObjectsRow> Rows = new List<ObjectsRow>();

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Name, Value FROM Objects",
                new Dictionary<string, object>()
            );

            if (result.Data != null && result.Data.HasRows)
            {
                while (result.Data.Read())
                {
                    // Handle NULL Value
                    if (result.Data.IsDBNull(1))
                    {
                        Rows.Add(new ObjectsRow(result.Data.GetString(0), string.Empty));
                    } else {
                        Rows.Add(new ObjectsRow(result.Data.GetString(0), result.Data.GetString(1)));    
                    }

                    result.Data.NextResult();
                }
            }

            return Rows;
        }

        /// <summary>
        /// Set the specified key of the objects.
        /// </summary>
        /// <param name="key">Object Key</param>
        /// <param name="newValue">Object Value</param>
        public static void Set(string key, string newValue)
        {
            key = Shared.Strings.Truncate(key, 128);
            newValue = Shared.Strings.Truncate(newValue, 128);

            Shared.Log.Message("DB", "Set " + key + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO Objects (Name, Value) VALUES (@Name, @Value)",
                new Dictionary<string, object>() {
                    {"@Name",key},
                    {"@Value",newValue},
                }
            );
        }
    }
}
