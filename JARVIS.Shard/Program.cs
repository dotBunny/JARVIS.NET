using System;
using System.IO;
using System.Threading;

namespace JARVIS.Shard
{
    class Program
    {
        public static Services.Socket.SocketClient Client = new Services.Socket.SocketClient();
        static ManualResetEvent QuitEvent = new ManualResetEvent(false);

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

            // Process commandline options
            CommandLineOptions options = new CommandLineOptions(args);

            // Assign options (defaulted as necessary)
            Client.Host = options.ServerHost;
            Client.Port = options.ServerSocketPort;
            Client.EncryptionKey = options.ServerSocketEncryptionKey;
            Client.Encryption = options.ServerSocketEncryption;
            Username = options.SessionUsername;
            Password = options.SessionPassword;
            OutputPath = options.OutputPath;
            HasCounterSupport = options.EnableCounters;
            HasWirecastSupport = options.EnableWirecast;

            // Display help/errors
            if (options.ShowHelp)
            {
                Environment.Exit(0);
            }

            // Get this party started!
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            // Start the connection to the server
            Client.Start();

            // Sit and wait till we get the CTRL-C
            QuitEvent.WaitOne();

            Quit(0);
        }

        public static void Quit(int code = 0){
            // Disconnect from the server
            Client.Stop();

            // Cleanly exit the program
            Shared.Log.Message("system", "Shutdown");
            Environment.Exit(code);
        }
    }

}
