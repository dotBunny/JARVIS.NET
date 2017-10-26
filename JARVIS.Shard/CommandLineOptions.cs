using System;
using Plossum.CommandLine;

namespace JARVIS.Shard
{
    [CommandLineManager(
        ApplicationName = "JARVIS Shard",
        Copyright = "",
        RequireExplicitAssignment = false,
        EnabledOptionStyles = OptionStyles.Group | OptionStyles.LongUnix)]
    [CommandLineOptionGroup("features", Name = "Features")]
    [CommandLineOptionGroup("settings", Name = "Settings")]
    class CommandLineOptions
    {
        bool enableCounterSupport = false;
        [CommandLineOption(Name = "counters", 
                           Description = "Enable Counter Support", 
                           Aliases="enable-counters", 
                           GroupId = "features")]
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
        [CommandLineOption(Name = "wirecast", 
                           Description = "Enable Wirecast Support", 
                           Aliases="enable-wirecast", 
                           GroupId = "features")]
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
        [CommandLineOption(Name = "output", 
                           Description = "Set output path for items that use file storage.", 
                           GroupId = "settings")]
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
                    throw new InvalidOptionValueException("You must provide a directory path.", false);
                }
                else if (!System.IO.Directory.Exists(value))
                {
                    var d = System.IO.Directory.CreateDirectory(value);

                    // Couldn't make the directory
                    if (!d.Exists)
                    {
                        throw new InvalidOptionValueException("The provided directory path did not exist, and failed to be made.", false);
                    }
                }
                else
                {
                    outputPath = value;
                }
            }
        }

        string serverSocketEncryptionKey = "max";
        [CommandLineOption(Name = "key", 
                           Description = "Encryption Key", 
                           GroupId = "settings")]
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
                    throw new InvalidOptionValueException("You must provide an encryption key.", false);
                }
                else
                {
                    serverSocketEncryptionKey = value;
                }
            }
        }

        string serverHost = "localhost";
        [CommandLineOption(Name = "host",
                           Description = "The hostname or the IP address of the JARVIS Server", 
                           GroupId = "settings")]
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
                }
            }
        }

        int serverSocketPort = 8081;
        [CommandLineOption(Name = "port", 
                           Description = "The port of the JARVIS Server",
                           GroupId = "settings")]
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
                }
            }
        }

        string sessionPassword = "jarvis";
        [CommandLineOption(Name = "password", 
                           Description = "JARVIS Password", 
                           GroupId = "settings")]
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
                    throw new InvalidOptionValueException("You must provide a password.", false);
                }
                else
                {
                    sessionPassword = value;
                }
            }
        }

        string sessionUsername = "shard";
        [CommandLineOption(Name = "username", 
                           Description = "JARVIS Username", 
                           GroupId = "settings")]
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
                    throw new InvalidOptionValueException("You must provide a username.", false);
                }
                else
                {
                    sessionUsername = value;
                }
            }
        }
    }
}