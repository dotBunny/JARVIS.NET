using System;
using Microsoft.Extensions.CommandLineUtils;

namespace JARVIS
{
    class CommandLineOptions
    {
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
                    //throw new InvalidOptionValueException("You must provide a hostname.", false);
                }
                else
                {
                    serverHost = value;
                    SetServerHost = true;
                }
            }
        }
        public bool SetServerHost { get; private set; }

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
                    //throw new InvalidOptionValueException("You must provide a valid port.", false);
                }
                else
                {
                    serverSocketPort = value;
                    SetSocketPort = true;
                }
            }
        }
        public bool SetSocketPort { get; private set; }
                           
        int serverWebPort = 1310;
        public int ServerWebPort
        {
            get
            {
                return serverWebPort;
            }
            set
            {
                if (value == 0)
                {
                    //throw new InvalidOptionValueException("You must provide a valid port.", false);
                }
                else
                {
                    serverWebPort = value;
                    SetWebPort = true;
                }
            }
        }
        public bool SetWebPort { get; private set; }

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

        bool quitAfter = false;
        public bool QuitAfter
        {
            get
            {
                return quitAfter;
            }
            set
            {
                quitAfter = value;
            }
        }

        public CommandLineOptions(string[] args)
        {
            // Create parser and don't barf if we get an unrecognized argument
            CommandLineApplication commandLine = new CommandLineApplication(false);

            CommandOption useHost = commandLine.Option("--host <IP>", "The hostname or the IP address of the JARVIS.Server", CommandOptionType.SingleValue);
            CommandOption useSocketPort = commandLine.Option("--socket-port <PORT>", "Sets the socket port of the JARVIS Server", CommandOptionType.SingleValue);
            CommandOption useWebPort = commandLine.Option("--web-port <PORT>", "Sets the web port of the JARVIS Server", CommandOptionType.SingleValue);
            CommandOption useQuit = commandLine.Option("--quit", "Quit after updating database with set values", CommandOptionType.NoValue);
            // Define help option
            commandLine.HelpOption("--help");


            commandLine.OnExecute(() =>
            {
                // If we have a host value
                if (useHost.HasValue())
                {
                    ServerHost = useHost.Value();
                    SetServerHost = true;
                }

                // If we have a socket port value
                if (useSocketPort.HasValue())
                {
                    int.TryParse(useSocketPort.Value().Trim(), out serverSocketPort);
                    SetSocketPort = true;
                }

                // If we have a socket port value
                if (useWebPort.HasValue())
                {
                    int.TryParse(useWebPort.Value().Trim(), out serverWebPort);
                    SetWebPort = true;
                }

                if (useQuit.HasValue())
                {
                    quitAfter = true;   
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