using System;
using Microsoft.Extensions.CommandLineUtils;
namespace JARVIS.Shard
{
    class CommandLineOptions
    {
        bool enableCounterSupport = false;
        public bool EnableCounters
        {
            get
            {
                return enableCounterSupport;
            }
            set
            {
                enableCounterSupport = value;
            }
        }

        bool enableWirecastSupport = false;
        public bool EnableWirecast
        {
            get
            {
                return enableWirecastSupport;
            }
            set
            {
                enableWirecastSupport = value;
            }
        }

        string outputPath = "./";
        public string OutputPath
        {
            get
            {
                return outputPath;
            }
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                 //   throw new InvalidOptionValueException("You must provide a directory path.", false);
                }
                else if (!System.IO.Directory.Exists(value))
                {
                    var d = System.IO.Directory.CreateDirectory(value);

                    // Couldn't make the directory
                    if (!d.Exists)
                    {
                   //     throw new InvalidOptionValueException("The provided directory path did not exist, and failed to be made.", false);
                    }
                }
                else
                {
                    outputPath = value;
                }
            }
        }

        bool showHelp = false;
        public bool ShowHelp
        {
            get
            {
                return showHelp;
            }
            set
            {
                showHelp = value;
            }
        }
        string serverSocketEncryptionKey = "max";
        public string ServerSocketEncryptionKey
        {
            get
            {
                return serverSocketEncryptionKey;
            }
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                  //  throw new InvalidOptionValueException("You must provide an encryption key.", false);
                }
                else
                {
                    serverSocketEncryptionKey = value;
                }
            }
        }

        string serverHost = "localhost";
        public string ServerHost
        {
            get
            {
                return serverHost;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                  //  throw new InvalidOptionValueException("You must provide a hostname.", false);
                }
                else
                {
                    serverHost = value;
                }
            }
        }

        int serverSocketPort = 665;
        public int ServerSocketPort
        {
            get
            {
                return serverSocketPort;
            }
            set
            {
                if (value == 0)
                {
                   // throw new InvalidOptionValueException("You must provide a valid port.", false);
                }
                else
                {
                    serverSocketPort = value;
                }
            }
        }

        string sessionPassword = "jarvis";
        public string SessionPassword
        {
            get 
            {
                return sessionPassword;
            }
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                   // throw new InvalidOptionValueException("You must provide a password.", false);
                }
                else
                {
                    sessionPassword = value;
                }
            }
        }

        string sessionUsername = "shard";
        public string SessionUsername
        {
            get 
            {
                return sessionUsername;
            }
            set
            {

                if (String.IsNullOrEmpty(value))
                {
                   //throw new InvalidOptionValueException("You must provide a username.", false);
                }
                else
                {
                    sessionUsername = value;
                }
            }
        }


        public CommandLineOptions(string[] args)
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
                    ServerHost = useHost.Value();
                }
                // If we have a port value
                if (usePort.HasValue())
                {
                    int.TryParse(usePort.Value().Trim(), out serverSocketPort);
                }

                if (useUsername.HasValue())
                {
                    SessionUsername = useUsername.Value();
                }

                if (usePassword.HasValue())
                {
                    SessionPassword = usePassword.Value();
                }

                // Handle output path setting
                if (useOutput.HasValue())
                {
                    OutputPath = useOutput.Value();

                }

                // Handle Counters
                if (useCounters.HasValue())
                {
                    EnableCounters = true;
                }

                // Update Wirecast
                if (useWirecast.HasValue())
                {
                    EnableWirecast = true;
                }

                // Handle Encryption Key
                if (useEncryptionKey.HasValue())
                {

                    ServerSocketEncryptionKey = useEncryptionKey.Value();
                }

                if (commandLine.OptionHelp.HasValue())
                {
                    ShowHelp = true;
                }

                return 0;
            });

            // Parse Arguments
            commandLine.Execute(args);
        }
    }
}