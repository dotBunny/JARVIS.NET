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
        public string SocketEncryptionKey = "max";
        public bool SocketEncryption = true;
        public int DatabaseVersion = 1;


        Dictionary<string, string> RawSettings = new Dictionary<string, string>();

        //public void SetHost(string host)
        //{
            
        //}
        //public void SetWebPort(string port)
        //{
            
        //}
        //public void SetSocketPort(string port)
        //{
            
        //}

        public string Get(string settingName)
        {
            if ( RawSettings.ContainsKey(settingName) ) {
                return RawSettings[settingName];
            } else {
                Shared.Log.Error("Settings", "Unable to find setting with key: " + settingName);
                return string.Empty;
            }
        }

        public void Save() {
            
        }


        public void Load() {


            if (!Server.Database.HasConnection) 
            {
                Shared.Log.Error("config", "Failed to load as database connection was not present.");
                return;
            }


            // Convert into dictionary
            List<Database.Rows.SettingsRow> rows = Database.Tables.Settings.GetAll();
            RawSettings.Clear();
            foreach (Database.Rows.SettingsRow r in rows)
            {
                RawSettings.Add(r.Name, r.Value);
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

            // Socket Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.DatabaseVersionID))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.DatabaseVersionID], out DatabaseVersion);
            }

            // Socket Encryption Key
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketEncryptionKeyID))
            {
                SocketEncryptionKey = RawSettings[Database.Tables.Settings.ServerSocketEncryptionKeyID];
            }
            // Socket Encryption
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketEncryptionID))
            {
                bool.TryParse(RawSettings[Database.Tables.Settings.ServerSocketEncryptionID], out SocketEncryption);
            }
        }
    }
}
