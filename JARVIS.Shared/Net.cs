using System;
using System.Net;

namespace JARVIS.Shared
{
    public static class Net
    {
        public const string SocketTerminator = "<EOL>";
        public const string SocketDeliminator = "||";
        public const string WebSuccessCode = "OK";
        public const string WebFailCode = "FAIL";

        public static string GetIPAddress(string hostname)
        {
            // Resolve hostname into IP of not IP
            IPHostEntry host = Dns.GetHostEntry(hostname);
            return host.AddressList[0].ToString();
        }

    }
}
