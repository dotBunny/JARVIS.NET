using AppKit;
using Foundation;
using JARVIS.Client.Mac.Windows;
using JARVIS.Shared.Protocol;

namespace JARVIS.Client.Mac
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public Services.Socket.SocketClient Client; 

        public MainWindowController mainWindowController { get; set; }
        PreferencesWindowController preferencesWindowController { get; set; }

        public Notifications NotificationsHandler { get; set; }


        public void OnConnecting()
        {
            // Server Menu
            ServerConnect.Enabled = false;
            ServerDisconnect.Enabled = true;

            ServicesSpotifyForceAuthentication.Enabled = false;
            ServicesDiscordForceAuthentication.Enabled = false;
            ServicesStreamlabsForceAuthentication.Enabled = false;
        }
        public void OnConnected()
        {
            // Server Menu
            ServerConnect.Enabled = false;
            ServerDisconnect.Enabled = true;

            ServicesSpotifyForceAuthentication.Enabled = true;
            ServicesDiscordForceAuthentication.Enabled = true;
            ServicesStreamlabsForceAuthentication.Enabled = true;
        }
        public void OnDisconnecting()
        {
            // Server Menu
            ServerConnect.Enabled = false;
            ServerDisconnect.Enabled = false;

            ServicesSpotifyForceAuthentication.Enabled = false;
            ServicesDiscordForceAuthentication.Enabled = false;
            ServicesStreamlabsForceAuthentication.Enabled = false;
        }
        public void OnDisconnected()
        {
            // Server Menu
            ServerConnect.Enabled = true;
            ServerDisconnect.Enabled = false;

            ServicesSpotifyForceAuthentication.Enabled = false;
            ServicesDiscordForceAuthentication.Enabled = false;
            ServicesStreamlabsForceAuthentication.Enabled = false;
        }


        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Initialize Our Notifications
            NotificationsHandler = new Notifications();

            Shared.Log.Notifier = NotificationsHandler;

            // Try To Connect - Pass in menu items for status updating
            Client = new Services.Socket.SocketClient(this);
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



        partial void OnServicesSpotifyForceAuthentication(AppKit.NSMenuItem sender)
        {
            if (Client != null && Client.IsConnected)
            {
                Client.Send(Shared.Protocol.Instruction.OpCode.AUTH_SPOTIFY);
            }
        }
        partial void OnServicesStreamlabsForceAuthentication(NSMenuItem sender)
        {
            if (Client != null && Client.IsConnected)
            {
                Client.Send(Shared.Protocol.Instruction.OpCode.AUTH_STREAMLABS);
            }
        }
        partial void OnServicesDiscordForceAuthentication(NSMenuItem sender)
        {
            if (Client != null && Client.IsConnected)
            {
                Client.Send(Shared.Protocol.Instruction.OpCode.AUTH_STREAMLABS);
            }
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
