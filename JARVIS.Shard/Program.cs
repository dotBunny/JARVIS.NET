using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;

namespace JARVIS.Shard
{
    class Program
    {
        private static ManualResetEvent QuitEvent = new ManualResetEvent(false);
        public static Services.Socket.SocketService Server = new Services.Socket.SocketService();

        public static string OutputPath = "";

        public static bool HasWirecastSupport = false;
        public static bool HasOutputSupport = false;

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

            // Determine server address / port
            Server = new Services.Socket.SocketService();


            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };

            Server.Start();


            QuitEvent.WaitOne();
            Server.Stop();

            Program.Shutdown();
        }

        static void ProcessCommandLine(string[] args)
        {
            // Create parser and don't barf if we get an unrecognized argument
            CommandLineApplication commandLine = new CommandLineApplication(false);

            CommandOption useOutput = commandLine.Option("--output <PATH>", "Enable output, and set the absolute path to output files too.", CommandOptionType.SingleValue);
            CommandOption useHost = commandLine.Option("--host <IP>", "The hostname or the IP address of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption usePort = commandLine.Option("--port <PORT>", "The port of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption useWirecast = commandLine.Option("--wirecast", "Enable Wirecast Support", CommandOptionType.NoValue);
            CommandOption useUsername = commandLine.Option("--username <USERNAME>", "JARVIS Username", CommandOptionType.SingleValue);
            CommandOption usePassword = commandLine.Option("--password <PASSWORD>", "JARVIS Password", CommandOptionType.SingleValue);


            // Define help option
            commandLine.HelpOption("--help");

            commandLine.OnExecute(() =>
            {
                // If we have a host value
                if (useHost.HasValue())
                {
                    Server.Host = useHost.Value();
                }
                // If we have a port value
                if (usePort.HasValue())
                {
                    int.TryParse(usePort.Value(), out Server.Port);
                }

                if (useUsername.HasValue())
                {
                    Username = useUsername.Value();
                }

                if (usePassword.HasValue())
                {
                    Password = usePassword.Value();
                }

                // Handle output path setting
                if (useOutput.HasValue())
                {
                    
                    if (Directory.Exists(useOutput.Value()))
                    {
                        HasOutputSupport = true;
                        OutputPath = useOutput.Value();
                    }
                    else
                    {
                        Shared.Log.Error("output", "The output directory must already exist.");
                        Program.Shutdown(1);
                    }
                }

                // Update Wirecast
                if (useWirecast.HasValue())
                {
                    HasWirecastSupport = true;
                }

                return 0;
            });

            // Parse Arguments
            commandLine.Execute(args);

            // Shutdown if were showing help
            if ( commandLine.IsShowingInformation ) {
                Program.Shutdown();
            }
        }

        public static void Shutdown(int exitCode = 0)
        {
            Shared.Log.Message("system", "Shutdown.");
            Environment.Exit(exitCode);
        }
    }

}
