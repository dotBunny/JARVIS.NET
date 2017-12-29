using AppKit;
using Foundation;
using JARVIS.Client.Mac.Windows;

namespace JARVIS.Client.Mac
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public Services.Socket.SocketClient Client; 

        public MainWindowController mainWindowController { get; set; }
        PreferencesWindowController preferencesWindowController { get; set; }

        public Notifications NotificationsHandler { get; set; }



        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Initialize Our Notifications
            NotificationsHandler = new Notifications();

            // Try To Connect - Pass in menu items for status updating
            Client = new Services.Socket.SocketClient(ServerConnect, ServerDisconnect);
            Client.Start();

        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application

            // Make sure that the Client is disconnected
            if ( Client != null && Client.IsConnected ) {
                Client.Stop();
            }
        }


        partial void OnJARVISPreferences(AppKit.NSMenuItem sender)
        {
            ShowPreferencesWindow();
        }

        partial void OnWindowShow(AppKit.NSMenuItem sender)
        {
           ShowMainWindow();
        }

        partial void OnWindowTestLogic(AppKit.NSMenuItem sender)
        {
            

            // Trigger a local notification after the time has elapsed
            var notification = new NSUserNotification();

            // Add text and sound to the notification
            notification.Title = "Authentication Required";
            notification.InformativeText = "The server needs you to approve its access to Spotify";
            notification.SoundName = NSUserNotification.NSUserNotificationDefaultSoundName;
            notification.HasActionButton = true;
            notification.HasReplyButton = false;
            notification.ActionButtonTitle = "Resolve";

            // Add data keys
            notification.UserInfo = NSDictionary.FromObjectAndKey(
                new NSString("Oauth CODE?"),
                new NSString("OP"));

            NotificationsHandler.Notify(notification);
        }

        partial void OnServerConnect(AppKit.NSMenuItem sender)
        {
            Client.Start();
        }

        partial void OnServerDisconnect(AppKit.NSMenuItem sender) 
        {
            
            Client.Stop();
        }



        void ShowMainWindow()
        {
            if (mainWindowController == null)
            {
                mainWindowController = new MainWindowController();
            }

            mainWindowController.Window.MakeKeyAndOrderFront(this);
        }
        void ShowPreferencesWindow()
        {
            if (preferencesWindowController == null)
            {
                preferencesWindowController = new PreferencesWindowController();
            }

            preferencesWindowController.Window.MakeKeyAndOrderFront(this);
        }
    }
}
