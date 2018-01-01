using System.IO;
using System.Collections.Generic;

namespace JARVIS.Shared
{
    /// <summary>
    /// Input/Output Related Helpers.
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Write the provided contents to a file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contents">The contents to be written to the file.</param>
        public static void WriteContents(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Write the provided contents to a file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contents">The contents to be written to the file.</param>
        public static void WriteContents(string filePath, byte[] contents)
        {
            File.WriteAllBytes(filePath, contents);
        }

        /// <summary>
        /// Appends the provided contents to a file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contents">The contents to be appended to the file.</param>
        public static void AppendContents(string filePath, string contents)
        {
            File.AppendAllText(filePath, contents);
        }

        /// <summary>
        /// Appends the provided contents to a file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="contents">The contents to be appended to the file.</param>
        public static void AppendContents(string filePath, List<string> contents)
        {
            File.AppendAllLines(filePath, contents);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
