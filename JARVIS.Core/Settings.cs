using System;
using System.Collections.Generic;

namespace JARVIS.Core
{
    public class Settings
    {
        public string DatabaseFilePath = "JARVIS.db";

        public int WebPort = 8080;
        public int SocketPort = 8081;
        public string Host = "localhost";


        public Dictionary<string, string> RawSettings = new Dictionary<string, string>();

        //public void SetHost(string host)
        //{
            
        //}
        //public void SetWebPort(string port)
        //{
            
        //}
        //public void SetSocketPort(string port)
        //{
            
        //}



        public void Save() {
            
        }


        public void Load(Database.Provider database ) {


            List<Database.Tables.Settings> settings = database.Connection.Query<Database.Tables.Settings>("SELECT * FROM \"" + Database.Tables.Settings.GetTableName() + "\"");

            // Convert into dictionary
            RawSettings.Clear();

            // Add to raw settings dictionary
            foreach(Database.Tables.Settings setting in settings) 
            {
                RawSettings.Add(setting.Name, setting.Value);
            }


            // Server Host Address
            if ( RawSettings.ContainsKey(Database.Tables.Settings.ServerHostID) )
            {
                Host = RawSettings[Database.Tables.Settings.ServerHostID];    
            }

            // Resolve hostname into IP of not IP
            Host = Shared.Net.GetIPAddress(Host);

            // Web Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerWebPortID))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.ServerWebPortID], out WebPort);
            }

            // Socket Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketPortID))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.ServerSocketPortID], out SocketPort);
            }
        }
    }
}
