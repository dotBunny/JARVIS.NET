using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace JARVIS
{
    class Program
    {
        public static int ProcessID = 0;

        public static string ProcessIDFilePath = "JARVIS.pid";

        /// <summary>
        /// The quit event listener
        /// </summary>
        static ManualResetEvent quitEvent = new ManualResetEvent(false);

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            // Capture Log
            Shared.Log.Capture(true);
             
            // Indicate that we're starting the party
            Shared.Log.Message("system", "Starting up ... ");

            // Create PID File
            ProcessID = Process.GetCurrentProcess().Id;
            ProcessIDFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), ProcessIDFilePath);
            Shared.IO.WriteContents(ProcessIDFilePath, ProcessID.ToString());
            Shared.Log.Message("system", "Process ID: " + ProcessID.ToString());

            // Process commandline and stop if we are showing stuff
            CommandLineOptions options = new CommandLineOptions(args);

            // Initialize Server
            Core.Server.Initialize();

            // Handle Custom SQL
            if ( options.HasSQLPath )
            {
                string sqlData = File.ReadAllText(options.SQLPath);
                Core.Server.Database.ExecuteNonQuery(sqlData, new System.Collections.Generic.Dictionary<string, object>());
            }

            // Handle special setting options from command line
            if (options.SetServerHost)
            {
                Core.Database.Tables.Settings.Set(
                  Core.Database.Tables.Settings.ServerHostID,
                    options.ServerHost);
            }

            if (options.SetSocketPort)
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
                Quit(0);
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
#if !DEBUG
            } 
            catch ( Exception e )
            {
                Shared.Log.Message("Exception", e.Message);
                Shared.Log.Message("Exception", e.Source);
                Shared.Log.Message("Exception", e.StackTrace);
                Quit(1);
            }
#endif
            // Exit
            Quit(0);
        }

        public static void Quit(int code)
        {
            Shared.Log.Message("System", "Good Bye!");
            Shared.IO.DeleteFile(ProcessIDFilePath);
            Shared.Log.WriteCache();
            Environment.Exit(code);
        }
    }
}
