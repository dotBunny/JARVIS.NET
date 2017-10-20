using System;
using System.Text;
using System.Threading;
using Grapevine.Server;

namespace JARVIS.Server
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
			Console.CancelKeyPress += (sender, eArgs) => {
				_quitEvent.Set();
				eArgs.Cancel = true;
			};

			using (var server = new RestServer())
			{
                server.Port = "8080";
				server.LogToConsole().Start();
				_quitEvent.WaitOne();
				server.Stop();
			}
        }

    }
}
