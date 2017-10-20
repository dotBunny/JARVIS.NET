using System;
using System.Net;
using System.Threading;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace JARVIS.Shard
{
    class Program
    {
        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static ServerConnection Server = new ServerConnection();

        public static void Main(string[] args)
        {

            // Startup 
            Shared.Log.Message("system", "Starting up ... ");

            // Determine server address / port
            Server = new ServerConnection();

            if ( args.Length > 0 ) 
            {
                
                Server.Host = args[0];
                if ( args.Length > 1 ) {
                    int.TryParse(args[1], out Server.Port);   
                }
            }

            Console.CancelKeyPress += (sender, eArgs) => {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            Server.Start();
            QuitEvent.WaitOne();
            Server.Stop();

            Shared.Log.Message("system", "Shutdown.");
        }
    }
}
