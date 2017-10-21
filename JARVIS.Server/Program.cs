using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;

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
            Shared.Log.Message("system", "Starting up ... ");

            // Default database path
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);

            // Handle Commandline Arguments
            CommandLineApplication commandLine = new CommandLineApplication(false);

            CommandOption useDatabase = commandLine.Option("--database <PATH>", "Absolute path to the SQLite database.", CommandOptionType.SingleValue);
            CommandOption useHost = commandLine.Option("--host <IP>", "Sets the hostname or the IP address of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption useSocketPort = commandLine.Option("--socket-port <PORT>", "Sets the socket port of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption useWebPort = commandLine.Option("--web-port <PORT>", "Sets the web port of the JARVIS.Server", CommandOptionType.SingleValue);

            // Define help option
            commandLine.HelpOption("--help");


            // What to do when processing arguments
            commandLine.OnExecute(() =>
            {
                // Handle output path setting
                if (useDatabase.HasValue())
                {
                    if (File.Exists(useDatabase.Value()))
                    {
                        Config.DatabaseFilePath = useDatabase.Value();
                    }
                }
                return 0;
            });

            // Parse Arguments
            commandLine.Execute(args);

            // Did we show help?
            if (!commandLine.IsShowingInformation)
            {
                Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);

                // Need to initialize database service before all else
                DB = new Database();
                DB.Start();

                // Handle special setting options from command line
                if (useHost.HasValue())
                {
                    //DB.Connection.InsertOrReplace()
                    // Set the host in the database?
                }
                if (useSocketPort.HasValue())
                {
                    // Set config port in the database?
                }
                if (useWebPort.HasValue())
                {
                    // Set config port in the database?
                }

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
                foreach (Services.IService service in Services)
                {
                    Shared.Log.Message("start", service.GetName() + " Service");
                    service.Start();
                }


                // This waits for CTRL-C, or whatever signal is sent to terminate the shell
                QuitEvent.WaitOne();


                // Stop Services
                foreach (Services.IService service in Services)
                {
                    Shared.Log.Message("stop", service.GetName() + " Service");
                    service.Stop();
                }
            }
        

            Shared.Log.Message("System", "Shutting down.");
        }

    }
}
