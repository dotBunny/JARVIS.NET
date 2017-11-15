using System.Collections.Generic;
using JARVIS.Core.Database.Rows;

namespace JARVIS.Core.Database.Tables
{
    /// <summary>
    /// JARVIS Counters Table
    /// </summary>
    public static class Counters
    {
        /// <summary>
        /// Get the specified counter from the Counters table.
        /// </summary>
        /// <returns>The specified row.</returns>
        /// <param name="name">The counter name.</param>
        public static CountersRow Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Value FROM Counters WHERE Name = @Name LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Name",name}
            }, System.Data.CommandBehavior.SingleResult);

            if ( result.Data != null && result.Data.HasRows ) 
            {
                result.Data.Read();
                return new CountersRow(name, result.Data.GetInt32(0));
            }

            return new CountersRow();
        }

        /// <summary>
        /// Set the specified value of the counter.
        /// </summary>
        /// <param name="name">Counter Name</param>
        /// <param name="newValue">Counter Value</param>
        public static void Set(string name, int newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set counter " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO Counters (Name, Value) VALUES (@Name, @Value)",
                new Dictionary<string, object>() {
                                {"@Name",name},
                                {"@Value",newValue},
                }
            );
        }
    }
}
