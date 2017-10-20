using System;
using System.Collections.Generic;

namespace JARVIS.Server
{
    public class Settings
    {
        public string DatabaseFilePath = "JARVIS.db";

        public string Port = "8080";
        public string Host = "localhost";

        public Dictionary<string, string> RawSettings = new Dictionary<string, string>();

        public void Save() {
            
        }
        public void Load() {


            List<Tables.Settings> settings = Program.DB.Database.Query<Tables.Settings>("SELECT * FROM \"" + Tables.Settings.GetTableName() + "\"");

            // Convert into dictionary
            RawSettings.Clear();

            // Add to raw settings dictionary
            foreach(Tables.Settings setting in settings) 
            {
                RawSettings.Add(setting.Name, setting.Value);
                Shared.Log.Message("settings", "Read " + setting.Name + " as " + setting.Value);
            }


            // Server Host Address
            if ( RawSettings.ContainsKey("Server.Host") )
            {
                Host = RawSettings["Server.Host"];    
            }

            // Server Port
            if (RawSettings.ContainsKey("Server.Port"))
            {
                Port = RawSettings["Server.Port"];
            }
        }
    }
}
