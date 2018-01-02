using System.Collections.Generic;
using JARVIS.Core.Database.Rows;

namespace JARVIS.Core.Database.Tables
{
    /// <summary>
    /// Key Value String Table
    /// </summary>
    public static class KeyValueString
    {
        /// <summary>
        /// Get the specified counter from the KeyValueString table.
        /// </summary>
        /// <returns>The specified row.</returns>
        /// <param name="name">The key.</param>
        public static KeyValueRow<string> Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Value FROM KeyValueString WHERE Name = @Name LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Name",name}
            }, System.Data.CommandBehavior.SingleResult);

            if (result.Data != null && result.Data.HasRows)
            {
                result.Data.Read();
                return new KeyValueRow<string>(name, result.Data.GetString(0));
            }

            return new KeyValueRow<string>();
        }

        /// <summary>
        /// Set the specified value of the KeyValueString.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="newValue">Value</param>
        public static void Set(string name, string newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set KeyValueInt " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO KeyValueString (Name, Value) VALUES (@Name, @Value)",
                new Dictionary<string, object>() {
                                {"@Name",name},
                                {"@Value",newValue},
                }
            );
        }
    }
}
