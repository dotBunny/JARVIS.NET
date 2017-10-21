using System;
using System.Collections.Generic;

namespace JARVIS.Server
{
    public class Settings
    {
        public string DatabaseFilePath = "JARVIS.db";

        public int WebPort = 8080;
        public int SocketPort = 8081;
        public string Host = "localhost";


        public Dictionary<string, string> RawSettings = new Dictionary<string, string>();

        public void Save() {
            
        }


        public void Load() {


            List<Tables.Settings> settings = Program.DB.Connection.Query<Tables.Settings>("SELECT * FROM \"" + Tables.Settings.GetTableName() + "\"");

            // Convert into dictionary
            RawSettings.Clear();

            // Add to raw settings dictionary
            foreach(Tables.Settings setting in settings) 
            {
                RawSettings.Add(setting.Name, setting.Value);
            }


            // Server Host Address
            if ( RawSettings.ContainsKey("Server.Host") )
            {
                Host = RawSettings["Server.Host"];    
            }

            // Resolve hostname into IP of not IP
            Host = Shared.Net.GetIPAddress(Host);

            // Web Port
            if (RawSettings.ContainsKey("Server.WebPort"))
            {
                int.TryParse(RawSettings["Server.WebPort"], out WebPort);
            }

            // Socket Port
            if (RawSettings.ContainsKey("Server.SocketPort"))
            {
                int.TryParse(RawSettings["Server.SocketPort"], out SocketPort);
            }
        }
    }
}
