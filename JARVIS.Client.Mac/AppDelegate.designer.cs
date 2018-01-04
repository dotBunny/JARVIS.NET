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

		[Outlet]
		AppKit.NSMenuItem ServicesDiscordForceAuthentication { get; set; }

		[Outlet]
		AppKit.NSMenuItem ServicesSpotifyForceAuthentication { get; set; }

		[Outlet]
		AppKit.NSMenuItem ServicesStreamlabsForceAuthentication { get; set; }

		[Action ("OnJARVISPreferences:")]
		partial void OnJARVISPreferences (AppKit.NSMenuItem sender);

		[Action ("OnServerConnect:")]
		partial void OnServerConnect (AppKit.NSMenuItem sender);

		[Action ("OnServerDisconnect:")]
		partial void OnServerDisconnect (AppKit.NSMenuItem sender);

		[Action ("OnServicesDiscordForceAuthentication:")]
		partial void OnServicesDiscordForceAuthentication (AppKit.NSMenuItem sender);

		[Action ("OnServicesSpotifyForceAuthentication:")]
		partial void OnServicesSpotifyForceAuthentication (AppKit.NSMenuItem sender);

		[Action ("OnServicesStreamlabsForceAuthentication:")]
		partial void OnServicesStreamlabsForceAuthentication (AppKit.NSMenuItem sender);

		[Action ("OnWindowShow:")]
		partial void OnWindowShow (AppKit.NSMenuItem sender);
		
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

			if (ServicesSpotifyForceAuthentication != null) {
				ServicesSpotifyForceAuthentication.Dispose ();
				ServicesSpotifyForceAuthentication = null;
			}

			if (ServicesDiscordForceAuthentication != null) {
				ServicesDiscordForceAuthentication.Dispose ();
				ServicesDiscordForceAuthentication = null;
			}

			if (ServicesStreamlabsForceAuthentication != null) {
				ServicesStreamlabsForceAuthentication.Dispose ();
				ServicesStreamlabsForceAuthentication = null;
			}
		}
	}
}
