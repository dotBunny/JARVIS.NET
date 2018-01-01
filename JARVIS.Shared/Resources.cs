using System.IO;
using System.Reflection;

namespace JARVIS.Shared
{
    /// <summary>
    /// Resources Related Helpers
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Get a <see cref="T:System.Byte"/> array of the resource from the target resource.
        /// </summary>
        /// <returns>The <see cref="T:System.Byte"/> array.</returns>
        /// <param name="name">The target resources name.</param>
        /// <param name="targetResource">The assembly name which the resource resides in (without extension).</param>
        public static byte[] GetBytes(string name, string targetResource = "JARVIS.Shared")
        {
            byte[] item;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(targetResource + "." + name))
            {
                item = new byte[stream.Length];
                stream.Read(item, 0,item.Length);
            
            }
            return item;
        }
    }
}