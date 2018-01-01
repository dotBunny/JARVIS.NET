using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace JARVIS.Client
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static bool EncryptionUseEncryptedProtocol
        {
            get
            {
                return AppSettings.GetValueOrDefault("EncryptionUseEncryptedProtocol", true);
            }
            set
            {
                AppSettings.AddOrUpdateValue("EncryptionUseEncryptedProtocol", value);
            }
        }

        public static string EncryptionServerEncryptionKey
        {
            get
            {
                return AppSettings.GetValueOrDefault("EncryptionServerEncryptionKey", "max");
            }
            set
            {
                AppSettings.AddOrUpdateValue("EncryptionServerEncryptionKey", value);
            }
        }

        public static bool FeatureUseNotifications
        {
            get
            {
                return AppSettings.GetValueOrDefault("FeatureUseNotifications", true);
            }
            set
            {
                AppSettings.AddOrUpdateValue("FeatureUseNotifications", value);
            }
        }

        public static bool FeatureFileOutputs
        {
            get
            {
                return AppSettings.GetValueOrDefault("FeatureFileOutputs", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("FeatureFileOutputs", value);
            }
        }

        public static string FeatureFileOutputsPath
        {
            get
            {
                return AppSettings.GetValueOrDefault("FeatureFileOutputsPath", Shared.Platform.GetBaseDirectory());
            }
            set
            {
                AppSettings.AddOrUpdateValue("FeatureFileOutputsPath", value);
            }
        }

        public static bool FeatureWirecastManipulation
        {
            get
            {
                return AppSettings.GetValueOrDefault("FeatureWirecastManipulation", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("FeatureWirecastManipulation", value);
            }
        }


        public static string ServerAddress
        {
            get 
            {
                return AppSettings.GetValueOrDefault("ServerAddress", Shared.Net.GetIPAddress("localhost"));
            }
            set
            {
                AppSettings.AddOrUpdateValue("ServerAddress", Shared.Net.GetIPAddress(value));
            }
        }


        public static int ServerPort
        {
            get
            {
                return AppSettings.GetValueOrDefault("ServerPort", 1311);
            }
            set
            {
                AppSettings.AddOrUpdateValue("ServerPort", value);
            }
        }

        public static string SessionPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault("SessionPassword", "jarvis");
            }
            set
            {
                AppSettings.AddOrUpdateValue("SessionPassword", value);
            }
        }

        public static string SessionUsername
        {
            get
            {
                return AppSettings.GetValueOrDefault("SessionUsername", "shard");
            }
            set
            {
                AppSettings.AddOrUpdateValue("SessionUsername", value);
            }
        }

    }
}
