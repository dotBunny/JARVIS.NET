namespace JARVIS.Shared
{
    /// <summary>
    /// Platform Related Helpers
    /// </summary>
    public static class Platform
    {
        /// <summary>
        /// Operating System
        /// </summary>
        public enum OperatingSystem
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Microsoft Windows Based
            /// </summary>
            Windows = 1,

            /// <summary>
            /// macOS Based
            /// </summary>
            macOS = 2,

            /// <summary>
            /// Linux Based
            /// </summary>
            Linux = 3
        }

        /// <summary>
        /// The cached base directory of the executing assembly.
        /// </summary>
        static string _cachedBaseDirectory;

        /// <summary>
        /// The cached operating system which the execution of the assembly is happening on.
        /// </summary>
        static OperatingSystem _cachedOperatingSystem = OperatingSystem.Unknown;

        /// <summary>
        /// The constant size of an integer in memory.
        /// </summary>
        public const int ByteSizeOfInt = sizeof(int);

        /// <summary>
        /// Gets the base directory of the executing assembly, and caches it.
        /// </summary>
        /// <returns>The base directory as a string.</returns>
        public static string GetBaseDirectory()
        {
            // This actually gets the library folder
            if (string.IsNullOrEmpty(_cachedBaseDirectory))
            {
                _cachedBaseDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:/", ""));
            }
            return _cachedBaseDirectory;
        }

        /// <summary>
        /// Get the Operating System which execution is currently happening on, and cache it.
        /// </summary>
        /// <returns>The os.</returns>
        public static OperatingSystem GetOS()
        {
            // Check for cache
            if ( _cachedOperatingSystem != OperatingSystem.Unknown ) {
                return _cachedOperatingSystem;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                _cachedOperatingSystem = OperatingSystem.macOS;

            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                _cachedOperatingSystem = OperatingSystem.Linux;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                _cachedOperatingSystem = OperatingSystem.Windows;
            }

            return _cachedOperatingSystem;
        }

        /// <summary>
        /// Run the specified executable with the provided arguments.
        /// </summary>
        /// <param name="executablePath">The path to the executable.</param>
        /// <param name="arguments">The arguments to pass to the executable.</param>
        /// <param name="hide">If set to <c>true</c> the application will attempt to run in teh background.</param>
        public static void Run(string executablePath, string arguments, bool hide)
        {
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = executablePath;
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.UseShellExecute = true;

            // Log infromation about executing command (a must!)
            Log.Message("OS", "Running " + executablePath + " " + arguments);

            // No need to show it
            if (hide)
            {
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
            }

			proc.Start();
		}
    }
}