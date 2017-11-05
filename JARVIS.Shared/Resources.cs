using System;
using System.IO;
using System.Reflection;
namespace JARVIS.Shared
{
    public static class Resources
    {
        
        public static byte[] GetBytes(string name)
        {
            byte[] item;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JARVIS.Shared." + name))
            {
                 item = new byte[stream.Length];
                stream.Read(item, 0,item.Length);
            
            }
            return item;
        }
    }
}