using System;
using System.IO;
using System.Threading;
using Plossum.CommandLine;

namespace JARVIS.Shard
{
    class Program
    {


        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        public static Services.Socket.SocketClient Client = new Services.Socket.SocketClient();


        // TODO: Add to settings file
        public static string OutputPath = "./";

        public static bool HasWirecastSupport = false;
        public static bool HasCounterSupport = false;

        public static string Username = "JARVIS";
        public static string Password = "demo";

        public static void Main(string[] args)
        {
            // Startup 
            Shared.Log.Message("system", "Starting up ... ");

            OutputPath = Path.Combine(Shared.Platform.GetBaseDirectory(), "Output");
            Directory.CreateDirectory(OutputPath);

            // Process commandline and stop if we are showing stuff
            CommandLineOptions options = new CommandLineOptions();
            CommandLineParser parser = new CommandLineParser(options);
            parser.Parse();

            // Assign options (defaulted as necessary)
            Client.Host = options.ServerHost;
            Client.Port = options.ServerSocketPort;
            Client.EncryptionKey = options.ServerSocketEncryptionKey;
            Username = options.SessionUsername;
            Password = options.SessionPassword;
            OutputPath = options.OutputPath;
            HasCounterSupport = options.EnableCounters;
            HasWirecastSupport = options.EnableWirecast;

            // Display help/errors
            if (parser.HasErrors)
            {
                Shared.Log.Message("help", parser.UsageInfo.ToString(78));
                Environment.Exit(-1);
            }

            // Get this party started!
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            // Start the connection to the server
            Client.Start().Wait();

            // Sit and wait till we get the CTRL-C
            QuitEvent.WaitOne();

            // Disconnect from the server
            Client.Stop();

            // Cleanly exit the program
            Shared.Log.Message("system", "Shutdown");
            Environment.Exit(0);
        }
    }

}
