using System;
using SQLite.Net;
namespace JARVIS.Server.Services
{
    public class DatabaseService : IService
    {
        public SQLiteConnection Database;

        public DatabaseService()
        {
          
        }

        ~DatabaseService()
        {
            Database.Dispose();
        }

        public void Start()
        {
            // Open database connection
            // TODO: Make platform specific calls
            Database = new SQLiteConnectionWithLock(
                new SQLite.Net.Platform.Generic.SQLitePlatformGeneric(),
                new SQLiteConnectionString(Program.Config.DatabaseFilePath, false));
            
        }

        public void Stop()
        {
            Database.Close();
        }
    }
}
