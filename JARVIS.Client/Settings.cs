using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace JARVIS.Client
{
    /// <summary>
    /// Client Settings
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The default settings value to use.
        /// </summary>
        static readonly string SettingsDefault = string.Empty;

        /// <summary>
        /// Get the current applications settings.
        /// </summary>
        /// <value>The applications settings.</value>
        static ISettings ApplicationSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// The encryption key to use in server communications, if using encryption.
        /// </summary>
        /// <value>The encryption key.</value>
        public static string EncryptionServerEncryptionKey
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("EncryptionServerEncryptionKey", "max");
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("EncryptionServerEncryptionKey", value);
            }
        }

        /// <summary>
        /// Should communications with the server be encrypted?
        /// </summary>
        /// <value><c>true</c> if encryption should be used; otherwise, <c>false</c>.</value>
        public static bool EncryptionUseEncryptedProtocol
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("EncryptionUseEncryptedProtocol", true);
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("EncryptionUseEncryptedProtocol", value);
            }
        }

        /// <summary>
        /// Should file outputs be done when received from the server?
        /// </summary>
        /// <value><c>true</c> if they should; otherwise, <c>false</c>.</value>
        public static bool FeatureFileOutputs
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("FeatureFileOutputs", false);
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("FeatureFileOutputs", value);
            }
        }

        /// <summary>
        /// The path where file outputs should occur.
        /// </summary>
        /// <value>The base output path of files being outputted.</value>
        public static string FeatureFileOutputsPath
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("FeatureFileOutputsPath", Shared.Platform.GetBaseDirectory());
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("FeatureFileOutputsPath", value);
            }
        }

        /// <summary>
        /// Should notifications be shown for events driven by the JARVIS server? 
        /// </summary>
        /// <value><c>true</c> for platform notifications; otherwise, <c>false</c>.</value>
        public static bool FeatureUseNotifications
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("FeatureUseNotifications", true);
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("FeatureUseNotifications", value);
            }
        }

        /// <summary>
        /// Should manipulations be done to Wirecast?
        /// </summary>
        /// <remarks>
        /// Currently only available on macOS
        /// </remarks>
        /// <value><c>true</c> to allow manipulation of Wirecast; otherwise, <c>false</c>.</value>
        public static bool FeatureWirecastManipulation
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("FeatureWirecastManipulation", false);
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("FeatureWirecastManipulation", value);
            }
        }

        /// <summary>
        /// The JARVIS core server address.
        /// </summary>
        /// <value>The server address to use.</value>
        public static string ServerAddress
        {
            get 
            {
                return ApplicationSettings.GetValueOrDefault("ServerAddress", Shared.Network.GetIPAddress("localhost"));
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("ServerAddress", Shared.Network.GetIPAddress(value));
            }
        }

        /// <summary>
        /// The JARVIS core server port.
        /// </summary>
        /// <value>The port to use.</value>
        public static int ServerPort
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("ServerPort", 1311);
            }
            set
            {
                if (Shared.Network.ValidatePort(value) == null)
                {
                    ApplicationSettings.AddOrUpdateValue("ServerPort", value);
                } 
                else 
                {
                    Shared.Log.Message("settings", "Invalid port number: " + value.ToString());
                }
            }
        }

        /// <summary>
        /// The client/session password to use when authenticating with the server.
        /// </summary>
        /// <value>The session password.</value>
        public static string SessionPassword
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("SessionPassword", "jarvis");
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("SessionPassword", value);
            }
        }

        /// <summary>
        /// The client/session username to use when authenticating with the server.
        /// </summary>
        /// <value>The session username.</value>
        public static string SessionUsername
        {
            get
            {
                return ApplicationSettings.GetValueOrDefault("SessionUsername", "shard");
            }
            set
            {
                ApplicationSettings.AddOrUpdateValue("SessionUsername", value);
            }
        }
    }
}
