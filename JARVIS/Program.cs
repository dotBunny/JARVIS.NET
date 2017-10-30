using System;
using System.Threading;

namespace JARVIS
{
    class Program
    {
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

            // Process commandline and stop if we are showing stuff
            CommandLineOptions options = new CommandLineOptions(args);

            // Initialize Server
            Core.Server.Initialize();

            // Handle special setting options from command line
            if (options.SetServerHost)
            {
                Core.Database.Tables.Settings.Set(
                  Core.Database.Tables.Settings.ServerHostID,
                    options.ServerHost);
            }

            if(options.SetSocketPort)
            {
                Core.Database.Tables.Settings.Set(
                    Core.Database.Tables.Settings.ServerSocketPortID,
                    options.ServerSocketPort.ToString());
            }
            if (options.SetWebPort)
            {
                Core.Database.Tables.Settings.Set(
                    Core.Database.Tables.Settings.ServerWebPortID,
                    options.ServerWebPort.ToString());
            }

            // Display help/errors
            if (options.ShowHelp || options.QuitAfter)
            {
                Environment.Exit(0);
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

            // Exit
            Shared.Log.Message("System", "Good Bye!");
            Environment.Exit(0);

        }
    }
}
