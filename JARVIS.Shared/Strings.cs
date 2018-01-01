using System;
using System.Text;

namespace JARVIS.Shared
{
    /// <summary>
    /// String Related Helpers
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Encode a string into bytes and get its Base64 string representation.
        /// </summary>
        /// <returns>The encoded string.</returns>
        /// <param name="source">The source string.</param>
        public static string Base64Encode(this string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Get the SHA256 hash of the string.
        /// </summary>
        /// <returns>The string hash.</returns>
        /// <param name="source">The source string.</param>
        public static string SHA256(this string source)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = System.Security.Cryptography.SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(source));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        /// <summary>
        /// Truncate the target string to a maximum length.
        /// </summary>
        /// <returns>The truncated string.</returns>
        /// <param name="target">The source string.</param>
        /// <param name="maximumLength">The maximum allowed length for the string.</param>
        public static string Truncate(this string target, int maximumLength)
        {
            if (string.IsNullOrEmpty(target)) return target;
            return target.Length <= maximumLength ? target : target.Substring(0, maximumLength);
        }
    }
}