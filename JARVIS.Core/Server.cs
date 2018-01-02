using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JARVIS.Core
{
    /// <summary>
    /// The core JARVIS server program
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// List Of Active Services
        /// </summary>
        public static ConcurrentBag<Services.IService> ActiveServices = new ConcurrentBag<Services.IService>();

        /// <summary>
        /// Settings Reference
        /// </summary>
        public static Settings Config;

        /// <summary>
        /// Database Reference
        /// </summary>
        public static Database.Provider Database;

        /// <summary>
        /// Socket Service Reference
        /// </summary>
        public static Services.Socket.SocketService Socket;

        /// <summary>
        /// Web Service Reference
        /// </summary>
        public static Services.Web.WebService Web;

        /// <summary>
        /// Discord Service Reference
        /// </summary>
        public static Services.Discord.DiscordService Discord;

        public static Services.Spotify.SpotifyService Spotify;


        static volatile bool ShouldTickFlag;

        static Thread PollingThread;


        public static int PollingDelayMS = 2000;

        public static void Initialize()
        {
            Config = new Settings();
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);

            Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);
            Database = new Database.Provider();
            Database.Open(Config.DatabaseFilePath);

            Shared.Log.Message("DB", "Opened version " + Database.Version);
            Shared.IO.WriteContents(Path.Combine(Shared.Platform.GetBaseDirectory(), "JARVIS.db.version"), Database.Version); 
                
        }

        public static void Start()
        {
            Shared.Log.Message("System", "Server Startup");

            // Load Config
            Config.Load();

            // Initialize Services
            Web = new Services.Web.WebService(Config.Host, Config.WebPort.ToString());
            Web.Start();
            ActiveServices.Add(Web);

            Socket = new Services.Socket.SocketService(Config.Host, Config.SocketPort, Config.SocketEncryption, Config.SocketEncryptionKey);
            Socket.Start();
            ActiveServices.Add(Socket);

            Shared.Log.Message("System", "Primary Startup Complete");


            // Start Secondary Services
            Spotify = new Services.Spotify.SpotifyService();
            Spotify.Start();
            ActiveServices.Add(Spotify);

            Discord = new Services.Discord.DiscordService();
            Discord.Start();
            ActiveServices.Add(Spotify);

            Shared.Log.Message("System", "Secondary Startup Complete");


            // Start Tick Thread
            Shared.Log.Message("System", "Starting Polling ...");
            ShouldTickFlag = true;
            PollingThread = new Thread(new ThreadStart(Tick));
            PollingThread.Name = "JARVIS-Polling";
            PollingThread.Start();



        }

        public static void Stop()
        {
            Shared.Log.Message("System", "Server Shutdown");

            Socket.Stop();
            Web.Stop();
        }

        static void Tick()
        {
            // This is the only one that actually evaluates it
            while (ShouldTickFlag)
            {
                Thread.Sleep(PollingDelayMS);

                // Threaded tick
                Parallel.ForEach(ActiveServices, s =>
                {
                    s.Tick();
                });
            }
        }



    }
}
