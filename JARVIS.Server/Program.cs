using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace JARVIS.Server
{
    class Program
    {

        public static Settings Config = new Settings();
        public static Database DB;

        public static Services.WebService Web;
        public static Services.SocketService Socket;

        public static List<Services.IService> Services = new List<Services.IService>();

        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);


        public static void Main(string[] args)
        {
            // Default database path
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);

            // Custom database path
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    Config.DatabaseFilePath = args[0];
                }
            }

            Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);

            // Need to initialize database service before all else
            DB = new Database();
            DB.Start();

            // Load our configuration values
            Config.Load();

            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            // Initialize Services
            Web = new Services.WebService();
            Socket = new Services.SocketService();

            // Start Services
            foreach (Services.IService service in Services){
                Shared.Log.Message("start", service.GetName()+" Service");
                service.Start();
            }
        

            // This waits for CTRL-C, or whatever signal is sent to terminate the shell
            QuitEvent.WaitOne();


            // Stop Services
            foreach (Services.IService service in Services){
                Shared.Log.Message("stop", service.GetName() + " Service");
                service.Stop();
            }
        

            Shared.Log.Message("System", "Shutting down.");
        }

    }
}
