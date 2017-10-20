using System;
using System.IO;
using System.Threading;


namespace JARVIS.Server
{
    class Program
    {

        public static Settings Config = new Settings();
        public static Services.WebService Web;
        public static Services.DatabaseService DB;

        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);


        public static void Main(string[] args)
        {
            // Default database path
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);    

            // Custom database path
            if ( args.Length > 0 ) {
                if (File.Exists(args[0])) {
                    Config.DatabaseFilePath = args[0];
                }
            }

            JARVIS.Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);

            // Need to initialize database service before all else
            DB = new Services.DatabaseService();
            DB.Start();

            // Load our configuration values
            Config.Load();

			Console.CancelKeyPress += (sender, eArgs) => {
				QuitEvent.Set();
				eArgs.Cancel = true;
			};

            // Initialize Services
            Web = new Services.WebService();

            // Start Services
            Web.Start();

            // This waits for CTRL-C, or whatever signal is sent to terminate the shell
            QuitEvent.WaitOne();

            JARVIS.Shared.Log.Message("System", "Shutting down ... ");

            // Stop Services
            Web.Stop();
        }

    }
}
