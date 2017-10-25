using System;
using System.Collections.Generic;
using System.Net;

namespace JARVIS.Shared
{
    public static class Net
    {
        public static string GetIPAddress(string hostname)
        {
            // Resolve hostname into IP of not IP
            IPHostEntry host = Dns.GetHostEntry(hostname);
            return host.AddressList[0].ToString();
        }
    }
}
