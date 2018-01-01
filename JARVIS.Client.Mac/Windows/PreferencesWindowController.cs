using System;

using Foundation;
using AppKit;

namespace JARVIS.Client.Mac.Windows
{
    public partial class PreferencesWindowController : NSWindowController
    {
        public PreferencesWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public PreferencesWindowController(NSCoder coder) : base(coder)
        {
        }

        public PreferencesWindowController() : base("PreferencesWindow")
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new PreferencesWindow Window
        {
            get { return (PreferencesWindow)base.Window; }
        }


        // Settings Tab

        partial void OnServerAddress(AppKit.NSTextField sender)
        {
            Settings.ServerAddress = sender.StringValue;
        }


        partial void OnServerPassword(AppKit.NSSecureTextField sender)
        {
            Settings.SessionPassword = sender.StringValue;
        }

        partial void OnServerPort(AppKit.NSTextField sender)
        {
            Settings.ServerPort = sender.IntValue;
        }

        partial void OnServerUsername(AppKit.NSTextField sender)
        {
            Settings.SessionUsername = sender.StringValue;
        }


        // Features Tab
        partial void OnFeaturesFileOutputs(AppKit.NSButton sender)
        {
            if (sender.State == NSCellStateValue.On)
            {
                Settings.FeatureFileOutputs = true;
            }
            else
            {
                Settings.FeatureFileOutputs = false;
            }
        }
        partial void OnFeaturesFileOutputsPath(AppKit.NSPathControl sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = false;
            dlg.CanChooseDirectories = true;

            if (dlg.RunModal() == 1)
            {
                // TODO: Validate path?
                Settings.FeatureFileOutputsPath = dlg.DirectoryUrl.Path;
                sender.StringValue = dlg.DirectoryUrl.Path;
            }

        }
        partial void OnFeaturesNotifications(AppKit.NSButton sender)
        {
            if (sender.State == NSCellStateValue.On)
            {
                Settings.FeatureUseNotifications = true;
            }
            else
            {
                Settings.FeatureUseNotifications = false;
            }
        }
        partial void OnFeaturesWirecastManipulation(AppKit.NSButton sender)
        {
            if (sender.State == NSCellStateValue.On)
            {
                Settings.FeatureWirecastManipulation = true;
            }
            else
            {
                Settings.FeatureWirecastManipulation = false;
            }
        }

        // Encryption Tab
        partial void OnEncryptionUseEncryptedProtocol(AppKit.NSButton sender) 
        {
            if ( sender.State == NSCellStateValue.On )
            {
                Settings.EncryptionUseEncryptedProtocol = true;
            } else {
                Settings.EncryptionUseEncryptedProtocol = false;
            }
        }
        partial void OnEncryptionServerEncryptionKey(AppKit.NSSecureTextField sender)
        {
            Settings.EncryptionServerEncryptionKey = sender.StringValue;
        }
    }
}
