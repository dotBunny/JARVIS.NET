using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JARVIS.Core
{
    /// <summary>
    /// The core JARVIS server program
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Settings Reference
        /// </summary>
        public static Settings Config;

        /// <summary>
        /// Database Reference
        /// </summary>
        public static Database.Provider Database;

        static ServiceCollection _serviceList;

        public static ServiceProvider Services;

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
            _serviceList = new ServiceCollection();

            // Create Socket Service
            _serviceList.AddSingleton(
                new Services.Socket.SocketService(
                    Config.Host, 
                    Config.SocketPort,
                    Config.SocketEncryption, 
                    Config.SocketEncryptionKey));

            // Create Web Service
            _serviceList.AddSingleton(
                new Services.Web.WebService(
                    Config.Host, 
                    Config.WebPort.ToString()));

            // Add Secondary Services
            _serviceList.AddSingleton(new Services.Spotify.SpotifyService())
                        .AddSingleton(new Services.Discord.DiscordService())
                        .AddSingleton(new Services.Streamlabs.StreamlabsService());

            // Create provider
            Services = _serviceList.BuildServiceProvider();

            // Start primary services ahead of everything else manaully
            Services.GetService<Services.Socket.SocketService>().Start();
            Services.GetService<Services.Web.WebService>().Start();
            Shared.Log.Message("System", "Primary Startup Complete");

            // Spin up any other service not fired up yet (spotify, discord, etc.)
            Services.GetService<Services.Spotify.SpotifyService>().Start();
            Services.GetService<Services.Discord.DiscordService>().Start();
            Services.GetService<Services.Streamlabs.StreamlabsService>().Start();

            // Start Tick Thread
            Shared.Log.Message("System", "Starting Polling ...");
            ShouldTickFlag = true;
            PollingThread = new Thread(new ThreadStart(Tick))
            {
                Name = "JARVIS-Polling"
            };
            PollingThread.Start();
        }

        public static void Stop()
        {
            Shared.Log.Message("System", "Server Shutdown");

            Services.GetService<Services.Discord.DiscordService>().Stop();
            Services.GetService<Services.Spotify.SpotifyService>().Stop();
            Services.GetService<Services.Web.WebService>().Stop();
            Services.GetService<Services.Socket.SocketService>().Stop();
        }

        static void Tick()
        {
            // This is the only one that actually evaluates it
            while (ShouldTickFlag)
            {
                Thread.Sleep(PollingDelayMS);

                // Threaded tick
                Services.GetService<Services.Spotify.SpotifyService>().Tick();
                Services.GetService<Services.Discord.DiscordService>().Tick();

            }
        }



    }
}
