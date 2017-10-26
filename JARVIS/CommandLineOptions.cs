using System;
using Plossum.CommandLine;

namespace JARVIS
{
    [CommandLineManager(
        ApplicationName = "JARVIS Server",
        Copyright = "",
        RequireExplicitAssignment = false,
        EnabledOptionStyles = OptionStyles.LongUnix)]

    class CommandLineOptions
    {
        string serverHost = "localhost";
        [CommandLineOption(Name = "host",
                           Aliases = "h,hostname",
                           Description = "The hostname or the IP address to listen on for connections.")]
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
                    throw new InvalidOptionValueException("You must provide a hostname.", false);
                }
                else
                {
                    serverHost = value;
                    SetServerHost = true;
                }
            }
        }
        public bool SetServerHost { get; private set; }

        int serverSocketPort = 8081;
        [CommandLineOption(Name = "socketport",
                           Aliases = "socket-port,s",
                           Description = "The socket port of the server")]
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
                    throw new InvalidOptionValueException("You must provide a valid port.", false);
                }
                else
                {
                    serverSocketPort = value;
                    SetSocketPort = true;
                }
            }
        }
        public bool SetSocketPort { get; private set; }
                           
        int serverWebPort = 8080;
        [CommandLineOption(Name = "webport",
                           Aliases = "web-port,w",
                           Description = "The web port of the server")]
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
                    throw new InvalidOptionValueException("You must provide a valid port.", false);
                }
                else
                {
                    serverWebPort = value;
                    SetWebPort = true;
                }
            }
        }
        public bool SetWebPort { get; private set; }

        bool quitAfter = false;
        [CommandLineOption(Name = "quit",
                           Description = "Force quit after initilization.",
                           Aliases = "q")]
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
    }
}