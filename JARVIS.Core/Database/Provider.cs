using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace JARVIS.Core.Database
{
    public class Provider
    {
        SqliteConnection Connection;
        SqliteConnectionStringBuilder ConnectionString;


        public bool HasConnection {
            get; private set;
        }

        ~Provider()
        {
            Connection.Close();
        }

        public void Open(string path)
        {

            // Open database connection
            // TODO: Make platform specific calls


            ConnectionString = new SqliteConnectionStringBuilder();
            ConnectionString.DataSource = path;


            Connection = new SqliteConnection(ConnectionString.ConnectionString);

            Connection.Open();

            if ( Connection.State == System.Data.ConnectionState.Closed )
            {
                HasConnection = false;
            } else {
                HasConnection = true;
            }

            // Wait for validation that the tables are there
            //Connection.CreateTableAsync<Tables.Settings>().Wait();
           // Connection.CreateTableAsync<Tables.Counters>().Wait();
        }

        public void Close()
        {
            Connection.Close();
            HasConnection = false;
        }

        public ProviderResult ExecuteQuery(string sql)
        {
            List<SqliteParameter> dummyParameters = new List<SqliteParameter>();
            Dictionary<string, object> dummyValues = new Dictionary<string, object>();

            return ExecuteQuery(sql, dummyParameters, dummyValues);
        }

        public ProviderResult ExecuteQuery(string sql,
                                           List<SqliteParameter> parameters,
                                           Dictionary<string, object> values,
                                           System.Data.CommandBehavior expected = System.Data.CommandBehavior.Default)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();
            result.Command.CommandText = sql;
            if (parameters.Count > 0)
            {
                result.Command.Parameters.AddRange(parameters);
            }
            foreach(KeyValuePair<string,object> item in values)
            {
                result.Command.Parameters[item.Key].Value = item.Value;
            }
            result.Data = result.Command.ExecuteReader(expected);
            return result;
        }

        public ProviderResult ExecuteQueryAsync(string sql, System.Data.CommandBehavior expected = System.Data.CommandBehavior.Default)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();
            result.Command.CommandText = sql;
            result.TaskQuery = result.Command.ExecuteReaderAsync(expected);
            result.IsAsync = true;
            return result;
        }

        public ProviderResult ExecuteNonQuery(string sql)
        {

            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            result.Return = result.Command.ExecuteNonQuery();
            return result;
        }

        public ProviderResult ExecuteNonQuery(string sql, Dictionary<string, object> replace)
        {

            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            foreach(KeyValuePair<string, object> item in replace)
            {
                result.Command.Parameters.AddWithValue(item.Key, item.Value);
            }

            result.Return = result.Command.ExecuteNonQuery();


            return result;
        }

        public ProviderResult ExecuteNonQueryAsyc(string sql)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            result.IsAsync = true;
            result.TaskNonQuery = result.Command.ExecuteNonQueryAsync();

            return result;

        }

        public ProviderResult ExecuteNonQueryAsyc(string sql, Dictionary<string, object> replace)
        {
            ProviderResult result = new ProviderResult();
            result.Command = Connection.CreateCommand();

            result.Command.CommandText = sql;

            foreach (KeyValuePair<string, object> item in replace)
            {
                result.Command.Parameters.AddWithValue(item.Key, item.Value);
            }

            result.IsAsync = true;
            result.TaskNonQuery = result.Command.ExecuteNonQueryAsync();

            return result;

        }

        public class ProviderResult
        {
            public SqliteCommand Command;
            public SqliteDataReader Data = null;
            public Task<int> TaskNonQuery;
            public Task<SqliteDataReader> TaskQuery;
            public bool IsAsync;
            public int Return;
        }

    }

   
}
