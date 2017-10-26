using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;

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
            ProcessCommandLine(args);

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

        static void ProcessCommandLine(string[] args)
        {
            // Create parser and don't barf if we get an unrecognized argument
            CommandLineApplication commandLine = new CommandLineApplication(false);

            CommandOption useOutput = commandLine.Option("--output <PATH>", "Set output path (Default: ./)", CommandOptionType.SingleValue);
            CommandOption useHost = commandLine.Option("--host <IP>", "The hostname or the IP address of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption usePort = commandLine.Option("--port <PORT>", "The port of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption useCounters = commandLine.Option("--counters", "Enable Counter Support", CommandOptionType.NoValue);
            CommandOption useWirecast = commandLine.Option("--wirecast", "Enable Wirecast Support", CommandOptionType.NoValue);
            CommandOption useUsername = commandLine.Option("--username <USERNAME>", "JARVIS Username", CommandOptionType.SingleValue);
            CommandOption usePassword = commandLine.Option("--password <PASSWORD>", "JARVIS Password", CommandOptionType.SingleValue);
            CommandOption useEncryptionKey = commandLine.Option("--key <ENCRYPTION_KEY>", "Encryption Key", CommandOptionType.SingleValue);

            // Define help option
            commandLine.HelpOption("--help");

            commandLine.OnExecute(() =>
            {
                // If we have a host value
                if (useHost.HasValue())
                {
                    Client.Host = useHost.Value().TrimEnd();
                }
                // If we have a port value
                if (usePort.HasValue())
                {
                    int.TryParse(usePort.Value().Trim(), out Client.Port);
                }

                if (useUsername.HasValue())
                {
                    Username = useUsername.Value();
                }

                if (usePassword.HasValue())
                {
                    Password = usePassword.Value().Trim();
                }

                // Handle output path setting
                if (useOutput.HasValue())
                {
                    if (Directory.Exists(useOutput.Value()))
                    {
                        OutputPath = useOutput.Value().Trim();
                    }
                }

                // Handle Counters
                if (useCounters.HasValue())
                {
                    HasCounterSupport = true;
                }

                // Update Wirecast
                if (useWirecast.HasValue())
                {
                    HasWirecastSupport = true;
                }

                // Handle Encryption Key
                if (useEncryptionKey.HasValue())
                {
                    Client.EncryptionKey = useEncryptionKey.Value().Trim();
                }

                return 0;
            });

            // Parse Arguments
            commandLine.Execute(args);

            // Shutdown if were showing help
            if ( commandLine.IsShowingInformation ) {
                Shared.Log.Message("system", "Quick Shutdown.");
                Environment.Exit(0);
            }
        }
    }

}
