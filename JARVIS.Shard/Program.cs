using System;
using System.Threading;

namespace JARVIS.Shard
{
    class Program
    {
        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static string ServerAddress = "localhost";

        public static void Main(string[] args)
        {

            // Startup 
            Shared.Log.Message("system", "Starting up ... ");


            // Determine server address
            if ( args.Length > 0 ) 
            {
                ServerAddress = args[0];    
            }

            // Connect to server



            Console.CancelKeyPress += (sender, eArgs) => {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };


            // Wait for commands




            // This waits for CTRL-C, or whatever signal is sent to terminate the shell
            QuitEvent.WaitOne();

            Shared.Log.Message("system", "Shutdown.");
        }
    }
}
