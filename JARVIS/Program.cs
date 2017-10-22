using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;

namespace JARVIS
{
    class Program
    {
        /// <summary>
        /// CommandLine parser
        /// </summary>
        private static CommandLineApplication commandLine = new CommandLineApplication(false);

        /// <summary>
        /// The database path command-line option
        /// </summary>
        private static CommandOption optionDatabase;

        /// <summary>
        /// The host command-line option
        /// </summary>
        private static CommandOption optionHost;

        /// <summary>
        /// The force quit after option execution of command-line option
        /// </summary>
        private static CommandOption optionQuit;

        /// <summary>
        /// The socket port command-line option
        /// </summary>
        private static CommandOption optionSocketPort;

        /// <summary>
        /// The web port command-line option
        /// </summary>
        private static CommandOption optionWebPort;

        /// <summary>
        /// The quit event listener
        /// </summary>
        private static ManualResetEvent quitEvent = new ManualResetEvent(false);

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Indicate that we're starting the party
            Shared.Log.Message("system", "Starting up ... ");


            // Handle command line arguments - sort of
            ProcessCommandLine(args);

            // Initialize Server
            Core.Server.Initialize();

            // Handle special setting options from command line
            if (optionHost.HasValue())
            {
                Core.Database.Tables.Settings.Set(
                    Core.Database.Tables.Settings.ServerHostID, 
                    optionHost.Value());
            }
            if (optionSocketPort.HasValue())
            {
                Core.Database.Tables.Settings.Set(
                    Core.Database.Tables.Settings.ServerSocketPortID, 
                    optionSocketPort.Value());
            }
            if (optionWebPort.HasValue())
            {
                Core.Database.Tables.Settings.Set(
                    Core.Database.Tables.Settings.ServerWebPortID, 
                    optionWebPort.Value());
            }

            // We have option'd to quit after setting values
            if (optionQuit.HasValue())
            {
                Shared.Log.Message("CLI", "Force Shutdown Detected.");
                Program.Shutdown();
            }


            // Start server
            Core.Server.Start();
     
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                quitEvent.Set();
                eArgs.Cancel = true;
            };

            // This waits for CTRL-C, or whatever signal is sent to terminate the shell
            quitEvent.WaitOne();

            // Stop server
            Core.Server.Stop();

            Program.Shutdown(0);

        }

        /// <summary>
        /// Processes the command line arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
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
                        Core.Server.Config.DatabaseFilePath = optionDatabase.Value();
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
            Shared.Log.Message("System", "Good Bye!");
            Environment.Exit(errorCode);
        }
    }
}
