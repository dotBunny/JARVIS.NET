using System.Collections.Generic;
using JARVIS.Core.Database.Rows;

namespace JARVIS.Core.Database.Tables
{
    /// <summary>
    /// Key Value Integer Table
    /// </summary>
    public static class KeyValueInt
    {
        /// <summary>
        /// Get the specified counter from the KeyValueInt table.
        /// </summary>
        /// <returns>The specified row.</returns>
        /// <param name="name">The key.</param>
        public static KeyValueRow<int> Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Value FROM KeyValueInt WHERE Name = @Name LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Name",name}
            }, System.Data.CommandBehavior.SingleResult);

            if ( result.Data != null && result.Data.HasRows ) 
            {
                result.Data.Read();
                return new KeyValueRow<int>(name, result.Data.GetInt32(0));
            }

            return new KeyValueRow<int>();
        }

        /// <summary>
        /// Set the specified value of the KeyValueInt.
        /// </summary>
        /// <param name="name">Key</param>
        /// <param name="newValue">Value</param>
        public static void Set(string name, int newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set KeyValueInt " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO KeyValueInt (Name, Value) VALUES (@Name, @Value)",
                new Dictionary<string, object>() {
                                {"@Name",name},
                                {"@Value",newValue},
                }
            );
        }
    }
}
