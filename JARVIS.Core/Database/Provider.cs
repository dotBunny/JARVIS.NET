using SQLite;

namespace JARVIS.Core.Database
{
    public class Provider
    {
        public SQLiteAsyncConnection Connection;

        public bool HasConnection {
            get; private set;
        }

        ~Provider()
        {
            Connection.GetConnection().Dispose();
        }

        public void Open(string path)
        {

            // Open database connection
            // TODO: Make platform specific calls
            Connection = new SQLiteAsyncConnection(path, false);

            if ( Connection != null ) {
                HasConnection = true;
            } else {
                HasConnection = false;   
            }

            // Wait for validation that the tables are there
            Connection.CreateTableAsync<Tables.Settings>().Wait();
            Connection.CreateTableAsync<Tables.Counters>().Wait();
        }

        public void Close()
        {
            Connection.GetConnection().Close();
            HasConnection = false;
        }
    }
}
