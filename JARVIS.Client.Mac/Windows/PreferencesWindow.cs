using System;
using Foundation;
using AppKit;

namespace JARVIS.Client.Mac.Windows
{
    public partial class PreferencesWindow : NSWindow
    {
        public PreferencesWindow(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public PreferencesWindow(NSCoder coder) : base(coder)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Server Settings
            ServerAddress.StringValue = Settings.ServerAddress;
            ServerPort.StringValue = Settings.ServerPort.ToString();
            ServerUsername.StringValue = Settings.SessionUsername;
            ServerPassword.StringValue = Settings.SessionPassword;

            // Features Settings
            if (Settings.FeatureUseNotifications) {
                FeaturesNotifications.State = NSCellStateValue.On;
            }

            if (Settings.FeatureFileOutputs) {
                FeaturesFileOutputs.State = NSCellStateValue.On;
            }

            FeaturesFileOutputsPath.StringValue = Settings.FeatureFileOutputsPath;

            if (Settings.FeatureWirecastManipulation)
            {
                FeaturesWirecastManipulation.State = NSCellStateValue.On;
            }

            // Encryption Settings
            if (Settings.EncryptionUseEncryptedProtocol)
            {
                EncryptionUseEncryptedProtocol.State = NSCellStateValue.On;    
            }
            EncryptionServerEncryptionKey.StringValue = Settings.EncryptionServerEncryptionKey;




        }
    }
}
