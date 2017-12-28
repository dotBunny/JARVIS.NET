// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JARVIS.Client.Mac.Windows
{
	[Register ("PreferencesWindowController")]
	partial class PreferencesWindowController
	{
		[Action ("OnEncryptionServerEncryptionKey:")]
		partial void OnEncryptionServerEncryptionKey (AppKit.NSSecureTextField sender);

		[Action ("OnEncryptionUseEncryptedProtocol:")]
		partial void OnEncryptionUseEncryptedProtocol (AppKit.NSButton sender);

		[Action ("OnFeaturesCounterOutputs:")]
		partial void OnFeaturesCounterOutputs (AppKit.NSButton sender);

		[Action ("OnFeaturesCounterOutputsPath:")]
		partial void OnFeaturesCounterOutputsPath (AppKit.NSPathControl sender);

		[Action ("OnFeaturesNotifications:")]
		partial void OnFeaturesNotifications (AppKit.NSButton sender);

		[Action ("OnFeaturesWirecastManipulation:")]
		partial void OnFeaturesWirecastManipulation (AppKit.NSButton sender);

		[Action ("OnServerAddress:")]
		partial void OnServerAddress (AppKit.NSTextField sender);

		[Action ("OnServerPassword:")]
		partial void OnServerPassword (AppKit.NSSecureTextField sender);

		[Action ("OnServerPort:")]
		partial void OnServerPort (AppKit.NSTextField sender);

		[Action ("OnServerUsername:")]
		partial void OnServerUsername (AppKit.NSTextField sender);
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
