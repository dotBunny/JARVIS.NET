


using SQLite.Net;

namespace JARVIS.Core.Database
{
    public class Provider
    {
        public SQLiteConnection Connection;

        public bool HasConnection {
            get; private set;
        }

        ~Provider()
        {
            Connection.Dispose();
        }

        public void Open(string path)
        {
            
            // Open database connection
            // TODO: Make platform specific calls
            Connection = new SQLiteConnectionWithLock(
                new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(),
                new SQLiteConnectionString(path, false));

            if ( Connection != null ) {
                HasConnection = true;
            } else {
                HasConnection = false;   
            }

            Connection.CreateTable<Tables.Settings>();
            Connection.CreateTable<Tables.Counters>();
        }

        public void Close()
        {
            Connection.Close();
            HasConnection = false;
        }
    }
}
