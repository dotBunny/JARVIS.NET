using System;
using System.Collections.Generic;

namespace JARVIS.Core
{
    public class Settings
    {
        public string DatabaseFilePath = "JARVIS.db";

        public int WebPort = 8080;
        public int SocketPort = 8081;
        public string Salt = "JARVIS";
        public string Host = "localhost";
        public string SocketEncryptionKey = "max";
        public bool SocketEncryption = true;
        public int DatabaseVersion = 1;

        public bool SpotifyEnabled = false;
        public string SpotifyClientID = "";
        public string SpotifyClientSecret = "";


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

        public bool GetBool(string settingName)
        {
            bool test = false;

            if (RawSettings.ContainsKey(settingName))
            {
                bool.TryParse(RawSettings[settingName], out test);
            }
            else
            {
                Shared.Log.Error("Settings", "Unable to find setting with key: " + settingName);
            }
            return test;
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
            List<Database.Rows.KeyValueRow<string>> rows = Database.Tables.Settings.GetAll();
            RawSettings.Clear();
            foreach (Database.Rows.KeyValueRow<string> r in rows)
            {
                RawSettings.Add(r.Name, r.Value);
            }

            // Server Host Address
            if ( RawSettings.ContainsKey(Database.Tables.Settings.ServerHostKey) )
            {
                Host = RawSettings[Database.Tables.Settings.ServerHostKey];    
            }

            // Resolve hostname into IP of not IP
            Host = Shared.Network.GetIPAddress(Host);

            // Web Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerWebPortKey))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.ServerWebPortKey], out WebPort);
            }

            // Socket Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketPortKey))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.ServerSocketPortKey], out SocketPort);
            }

            // Socket Port
            if (RawSettings.ContainsKey(Database.Tables.Settings.DatabaseVersionKey))
            {
                int.TryParse(RawSettings[Database.Tables.Settings.DatabaseVersionKey], out DatabaseVersion);
            }

            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSaltKey))
            {
                Salt = RawSettings[Database.Tables.Settings.ServerSaltKey];
            }

            // Socket Encryption Key
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketEncryptionKeyKey))
            {
                SocketEncryptionKey = RawSettings[Database.Tables.Settings.ServerSocketEncryptionKeyKey];
            }
            // Socket Encryption
            if (RawSettings.ContainsKey(Database.Tables.Settings.ServerSocketEncryptionKey))
            {
                bool.TryParse(RawSettings[Database.Tables.Settings.ServerSocketEncryptionKey], out SocketEncryption);
            }




            // List Current Settings
            foreach (KeyValuePair<string, string> setting in RawSettings)
            {
                Shared.Log.Message("Setting", setting.Key + ": " + setting.Value);
            }

        }
    }
}
