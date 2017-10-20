using System;
using SQLite.Net;
namespace JARVIS.Server
{
    public class Database
    {
        public SQLiteConnection Connection;

        ~Database()
        {
            Connection.Dispose();
        }

        public void Start()
        {
            // Open database connection
            // TODO: Make platform specific calls
            Connection = new SQLiteConnectionWithLock(
                new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(),
                new SQLiteConnectionString(Program.Config.DatabaseFilePath, false));


            Connection.CreateTable<Tables.Settings>();
        }

        public void Stop()
        {
            Connection.Close();
        }
    }
}
