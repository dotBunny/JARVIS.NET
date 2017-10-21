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

        // Command Line
        private static CommandLineApplication commandLine = new CommandLineApplication(false);
        private static CommandOption optionDatabase;
        private static CommandOption optionHost;
        private static CommandOption optionSocketPort;
        private static CommandOption optionWebPort;
        private static CommandOption optionQuit;

        public static void Main(string[] args)
        {
            Shared.Log.Message("system", "Starting up ... ");

            // Default database path
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);

            ProcessCommandLine(args);

            Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);

            // Need to initialize database service before all else
            DB = new Database();
            DB.Start();

            // Handle special setting options from command line
            if (optionHost.HasValue())
            {
                Shared.Log.Message("DB", "Setting Server.Host: " + optionHost.Value());
                DB.Connection.InsertOrReplace(new Tables.Settings()
                {
                    Name = "Server.Host",
                    Value = optionHost.Value()
                });
            }
            if (optionSocketPort.HasValue())
            {
                Shared.Log.Message("DB", "Setting Server.SocketPort: " + optionSocketPort.Value());
                DB.Connection.InsertOrReplace(new Tables.Settings()
                {
                    Name = "Server.SocketPort",
                    Value = optionSocketPort.Value()
                });

            }
            if (optionWebPort.HasValue())
            {
                Shared.Log.Message("DB", "Setting Server.WebPort: " + optionWebPort.Value());
                DB.Connection.InsertOrReplace(new Tables.Settings()
                {
                    Name = "Server.WebPort",
                    Value = optionWebPort.Value()
                });
            }

            // We have option'd to quit after setting values
            if (optionQuit.HasValue())
            {
                Program.Shutdown();
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

            Program.Shutdown(0);

        }

        static void ProcessCommandLine(string[] args)
        {
            // Setup Command Options
            Program.optionDatabase = commandLine.Option("--database <PATH>", "Absolute path to the SQLite database.", CommandOptionType.SingleValue);
            Program.optionHost = commandLine.Option("--host <IP>", "Sets the hostname or the IP address of the JARVIS.Server", CommandOptionType.SingleValue);
            Program.optionSocketPort = commandLine.Option("--socket-port <PORT>", "Sets the socket port of the JARVIS.Server", CommandOptionType.SingleValue);
            Program.optionWebPort = commandLine.Option("--web-port <PORT>", "Sets the web port of the JARVIS.Server", CommandOptionType.SingleValue);
            Program.optionQuit = commandLine.Option("--quit", "Quits application after evaluating commandline options (useful for just updating the database settings)", CommandOptionType.NoValue);

            // Define help option
            commandLine.HelpOption("--help");

            // What to do when processing arguments
            commandLine.OnExecute(() =>
            {
                // Handle output path setting
                if (optionDatabase.HasValue())
                {
                    if (File.Exists(optionDatabase.Value()))
                    {
                        Config.DatabaseFilePath = optionDatabase.Value();
                    }
                }
                return 0;
            });

            // Parse Arguments
            commandLine.Execute(args);

            if (commandLine.IsShowingInformation )
            {
                Program.Shutdown();
            }
        }

        static void Shutdown(int errorCode = 0)
        {
            Shared.Log.Message("System", "Shutting down.");
            Environment.Exit(errorCode);
        }
    }
}
