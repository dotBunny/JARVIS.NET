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
	[Register ("PreferencesWindow")]
	partial class PreferencesWindow
	{
		[Outlet]
		AppKit.NSSecureTextField EncryptionServerEncryptionKey { get; set; }

		[Outlet]
		AppKit.NSButton EncryptionUseEncryptedProtocol { get; set; }

		[Outlet]
		AppKit.NSButton FeaturesCounterOutputs { get; set; }

		[Outlet]
		AppKit.NSPathControl FeaturesCounterOutputsPath { get; set; }

		[Outlet]
		AppKit.NSButton FeaturesNotifications { get; set; }

		[Outlet]
		AppKit.NSButton FeaturesWirecastManipulation { get; set; }

		[Outlet]
		AppKit.NSTextField ServerAddress { get; set; }

		[Outlet]
		AppKit.NSSecureTextField ServerPassword { get; set; }

		[Outlet]
		AppKit.NSTextField ServerPort { get; set; }

		[Outlet]
		AppKit.NSTextField ServerUsername { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (EncryptionUseEncryptedProtocol != null) {
				EncryptionUseEncryptedProtocol.Dispose ();
				EncryptionUseEncryptedProtocol = null;
			}

			if (EncryptionServerEncryptionKey != null) {
				EncryptionServerEncryptionKey.Dispose ();
				EncryptionServerEncryptionKey = null;
			}

			if (FeaturesNotifications != null) {
				FeaturesNotifications.Dispose ();
				FeaturesNotifications = null;
			}

			if (FeaturesCounterOutputs != null) {
				FeaturesCounterOutputs.Dispose ();
				FeaturesCounterOutputs = null;
			}

			if (FeaturesCounterOutputsPath != null) {
				FeaturesCounterOutputsPath.Dispose ();
				FeaturesCounterOutputsPath = null;
			}

			if (FeaturesWirecastManipulation != null) {
				FeaturesWirecastManipulation.Dispose ();
				FeaturesWirecastManipulation = null;
			}

			if (ServerAddress != null) {
				ServerAddress.Dispose ();
				ServerAddress = null;
			}

			if (ServerPort != null) {
				ServerPort.Dispose ();
				ServerPort = null;
			}

			if (ServerUsername != null) {
				ServerUsername.Dispose ();
				ServerUsername = null;
			}

			if (ServerPassword != null) {
				ServerPassword.Dispose ();
				ServerPassword = null;
			}
		}
	}
}
