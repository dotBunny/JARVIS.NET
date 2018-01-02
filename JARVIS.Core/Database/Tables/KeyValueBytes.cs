using System.Collections.Generic;
using JARVIS.Core.Database.Rows;

namespace JARVIS.Core.Database.Tables
{
    /// <summary>
    /// Key Value Bytes Table
    /// </summary>
    public static class KeyValueBytes
    {
        /// <summary>
        /// Get the specified counter from the KeyValueBytes table.
        /// </summary>
        /// <returns>The specified row.</returns>
        /// <param name="name">The key.</param>
        public static KeyValueRow<byte[]> Get(string name)
        {
            // Max length
            name = Shared.Strings.Truncate(name, 128);

            Provider.ProviderResult result = Server.Database.ExecuteQuery(
                "SELECT Size, Value FROM KeyValueBytes WHERE Name = @Name LIMIT 1",
                new Dictionary<string, object>() {
                    {"@Name",name}
            }, System.Data.CommandBehavior.SingleResult);

            if ( result.Data != null && result.Data.HasRows ) 
            {
                result.Data.Read();

                // Get our datas
                int sizeOfData = result.Data.GetInt32(0);

                if (sizeOfData > 0)
                {
                    byte[] readData = new byte[sizeOfData];
                    result.Data.GetBytes(1, 0, readData, 0, sizeOfData);
                    return new KeyValueRow<byte[]>(name, readData);
                }
                return new KeyValueRow<byte[]>(name, null);
            }

            return new KeyValueRow<byte[]>();
        }

        /// <summary>
        /// Set the specified value of the KeyValueBytes.
        /// </summary>
        /// <param name="name">Key</param>
        /// <param name="newValue">Value</param>
        public static void Set(string name, byte[] newValue)
        {
            name = Shared.Strings.Truncate(name, 128);

            Shared.Log.Message("DB", "Set KeyValueBytes " + name + " to " + newValue);

            Server.Database.ExecuteNonQuery(
                "REPLACE INTO KeyValueBytes (Name, Value) VALUES (@Name, @Value)",
                new Dictionary<string, object>() {
                                {"@Name",name},
                                {"@Value",newValue},
                }
            );
        }
    }
}
