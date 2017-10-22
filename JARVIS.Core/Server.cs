using System;
using System.Collections.Generic;
using System.IO;

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
        public static List<Services.IService> ActiveServices = new List<Services.IService>();

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


        public static void Initialize()
        {
            Config = new Settings();
            Config.DatabaseFilePath = Path.Combine(Shared.Platform.GetBaseDirectory(), Config.DatabaseFilePath);

            Shared.Log.Message("DB", "Opening database at " + Config.DatabaseFilePath);
            Database = new Database.Provider();
            Database.Open(Config.DatabaseFilePath);
        }

        public static void Start()
        {
            // Load Config
            Config.Load(Database);

            //// Initialize Services
            Web = new Services.Web.WebService(Config.Host, Config.WebPort.ToString());
            ActiveServices.Add(Web);

            Socket = new Services.Socket.SocketService(Config.SocketPort);
            ActiveServices.Add(Socket);
        }

        public static void Stop()
        {
            Socket.Stop();
            Web.Stop();
        }


    }
}
