using System;
using System.Threading;
using Grapevine.Server;

namespace JARVIS.Server
{
    class Program
    {
        public static string Port = "8080";
        public static string Database = "JARVIS.db";
        public static bool HasCustomDatabase = false;
        public static bool HasCustomPort = false;

        static ManualResetEvent _quitEvent = new ManualResetEvent(false);


        public static void Main(string[] args)
        {
			// Handle Arguments
			switch ( args.Length ) {
                case 2:
					Program.Port = args[0];
                    Program.HasCustomPort = true;
					Program.Database = args[1];
                    Program.HasCustomDatabase = true;
                    break;
                case 1:
                    Program.Port = args[0];
                    Program.HasCustomPort = true;
                    break;
            }
          
			Console.CancelKeyPress += (sender, eArgs) => {
				_quitEvent.Set();
				eArgs.Cancel = true;
			};

			using (var server = new RestServer())
			{
                server.Port = Program.Port;
				server.LogToConsole().Start();
				_quitEvent.WaitOne();
				server.Stop();
			}
        }

    }
}
