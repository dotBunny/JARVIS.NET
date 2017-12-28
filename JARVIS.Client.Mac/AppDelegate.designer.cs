// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JARVIS.Client.Mac
{
    partial class AppDelegate
    {
        [Outlet]
        AppKit.NSMenuItem ServerConnect { get; set; }

        [Outlet]
        AppKit.NSMenuItem ServerDisconnect { get; set; }

        [Action ("OnJARVISPreferences:")]
        partial void OnJARVISPreferences (AppKit.NSMenuItem sender);

        [Action ("OnServerConnect:")]
        partial void OnServerConnect (AppKit.NSMenuItem sender);

        [Action ("OnServerDisconnect:")]
        partial void OnServerDisconnect (AppKit.NSMenuItem sender);

        [Action ("OnWindowShow:")]
        partial void OnWindowShow (AppKit.NSMenuItem sender);

        [Action ("OnWindowTestLogic:")]
        partial void OnWindowTestLogic (AppKit.NSMenuItem sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (ServerConnect != null) {
                ServerConnect.Dispose ();
                ServerConnect = null;
            }

            if (ServerDisconnect != null) {
                ServerDisconnect.Dispose ();
                ServerDisconnect = null;
            }
        }
    }
}
