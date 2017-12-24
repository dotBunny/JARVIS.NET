using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Eto;
using Eto.Forms;

namespace JARVIS.Client
{
    class Program
    {
        public static Eto.Forms.Application App;

        [STAThread]
        public static void Main()
        {
            switch(Shared.Platform.GetOS())
            {
                case Shared.Platform.OperatingSystem.Windows:
                    App = new Eto.Forms.Application(Eto.Platforms.WinForms);
                    break;
                case Shared.Platform.OperatingSystem.macOS:
                    App = new Eto.Forms.Application(Eto.Platforms.Mac);
                    break;
                case Shared.Platform.OperatingSystem.Linux:
                    App = new Eto.Forms.Application(Eto.Platforms.Gtk3);
                    break;
                default:
                    Shared.Log.Error("system", "Unsupported GUI Platform");
                    break;
            }
           

            // Launch this puppy
            if (App != null)
            {
                App.Run(new MainForm());
            }
        }
    }

}
