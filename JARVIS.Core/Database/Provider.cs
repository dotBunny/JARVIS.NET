using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace JARVIS.Core.Database
{
    /// <summary>
    /// SQLite Database Provider
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// The version of the database schema.
        /// </summary>
        public string Version;

        /// <summary>
        /// Was a connection made to the database.
        /// </summary>
        /// <value><c>true</c> if a connection was made; otherwise, <c>false</c>.</value>
        public bool HasConnection
        {
            get; private set;
        }

        /// <summary>
        /// Local reference to the database.
        /// </summary>
        SqliteConnection Connection;

        /// <summary>
        /// The connection string used to connect to the database.
        /// </summary>
        SqliteConnectionStringBuilder ConnectionString;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:JARVIS.Core.Database.Provider"/> is reclaimed by garbage collection.
        /// </summary>
        ~Provider()
        {
            Connection.Close();
        }

        /// <summary>
        /// Close connection to database.
        /// </summary>
        public void Close()
        {
            Connection.Close();
            HasConnection = false;
        }

        /// <summary>
        /// Executes a non-returning query synchronously.
        /// </summary>
        /// <returns>The result object.</returns>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">Parameters List</param>
        public ProviderResult ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            foreach (KeyValuePair<string, object> item in parameters)
            {
                result.Command.Parameters.AddWithValue(item.Key, item.Value);
            }


            result.Return = result.Command.ExecuteNonQuery();

            return result;
        }

        /// <summary>
        /// Executes a non-returning query asynchronously.
        /// </summary>
        /// <returns>The result object.</returns>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">Parameters List</param>
        public ProviderResult ExecuteNonQueryAsyc(string sql, Dictionary<string, object> parameters)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            foreach (KeyValuePair<string, object> item in parameters)
            {
                result.Command.Parameters.AddWithValue(item.Key, item.Value);
            }

            result.IsAsync = true;
            result.TaskNonQuery = result.Command.ExecuteNonQueryAsync();

            return result;
        }

        /// <summary>
        /// Executes a returning query synchronously
        /// </summary>
        /// <returns>The result object.</returns>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">Parameters List</param>
        /// <param name="expected">Expected Behaviour</param>
        public ProviderResult ExecuteQuery(string sql,
                                            Dictionary<string, object> parameters,
                                            System.Data.CommandBehavior expected = System.Data.CommandBehavior.Default)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();
            result.Command.CommandText = sql;


            foreach (KeyValuePair<string, object> item in parameters)
            {
                result.Command.Parameters.AddWithValue(item.Key, item.Value);
            }

            result.Data = result.Command.ExecuteReader(expected);
            return result;
        }

        /// <summary>
        /// Open Database at the path provided.
        /// </summary>
        /// <param name="path">Path to database SQLite database.</param>
        public void Open(string path)
        {
            ConnectionString = new SqliteConnectionStringBuilder();
            ConnectionString.DataSource = path;


            Connection = new SqliteConnection(ConnectionString.ConnectionString);

            Connection.Open();

            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                HasConnection = false;
            }
            else
            {
                HasConnection = true;
            }

            string tempVersion = Tables.Settings.Get(Tables.Settings.DatabaseVersionKey).Value;
            if (tempVersion != string.Empty)
            {
                Version = tempVersion;
            }
        }

        /// <summary>
        /// The Database Provider Result
        /// </summary>
        public class ProviderResult
        {
            /// <summary>
            /// The issued command
            /// </summary>
            public SqliteCommand Command;

            /// <summary>
            /// The resulting data from the command
            /// </summary>
            public SqliteDataReader Data = null;

            /// <summary>
            /// A reference to an asynchronus non-returning task
            /// </summary>
            public Task<int> TaskNonQuery;

            /// <summary>
            /// A reference to an asynchronus returning task
            /// </summary>
            public Task<SqliteDataReader> TaskQuery;

            /// <summary>
            /// Is the result from an asynchronus task
            /// </summary>
            public bool IsAsync;

            /// <summary>
            /// Non asynchronus task return
            /// </summary>
            public int Return;

            ~ProviderResult()
            {
                if (Data != null) Data.Close();
                Command.Dispose();
            }
        }
    }

   
}
