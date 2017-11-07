using System;
namespace JARVIS.Shared
{
    public static class Platform
    {
        private static string CachedBaseDirectory;

        public static void Run(string path, string arguments, bool hide)
        {
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.UseShellExecute = true;

            // Log infromation about executing command (a must!)
            Log.Message("OS", "Running " + path + " " + arguments);

            // No need to show it
            if (hide)
            {
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
            }

			proc.Start();
		}

        public static string GetBaseDirectory()
        {
            // This actually gets the library folder
            if ( string.IsNullOrEmpty(CachedBaseDirectory) ) {
                CachedBaseDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:/", ""));
            }
            return CachedBaseDirectory;
        }
    }
}