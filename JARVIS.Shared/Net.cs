using System;
using System.Collections.Generic;
using System.Net;

namespace JARVIS.Shared
{
    public static class Net
    {
        public static string GetIPAddress(string hostname)
        {
            IPAddress address;
            if (IPAddress.TryParse(hostname, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        return hostname;
                }
            }

            IPHostEntry host = Dns.GetHostEntry(hostname);
            string returnHost = host.AddressList[0].ToString();
            if ( returnHost == "::1")
            {
                returnHost = "127.0.0.1";
            }

            return returnHost;
        }

        /// <summary>
        /// Validates the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static Exception ValidatePort(int port)
        {
            if (port >= 0 && port <= 65535)
            {
                return null;
            }
            else
            {
                return new FormatException("port must be greater than 0 and less than 65535");
            }
        }

        /// <summary>
        /// Validates the host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns></returns>
        public static Exception ValidateHost(string host)
        {
            if (string.IsNullOrEmpty(host) || host.Trim() == "")
            {
                return new ArgumentNullException(nameof(host));
            }

            IPAddress address = null;
            if (IPAddress.TryParse(host, out address))
            {
                return null;
            }
            else
            {
                return new FormatException("host format is incorrect");
            }
        }
    }
}
