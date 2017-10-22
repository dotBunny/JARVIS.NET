


using SQLite.Net;

namespace JARVIS.Core.Database
{
    public class Provider
    {
        public SQLiteConnection Connection;

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


            Connection.CreateTable<Tables.Settings>();
            Connection.CreateTable<Tables.Counters>();
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
