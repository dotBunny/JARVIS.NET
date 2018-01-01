using System;
using System.Net;

namespace JARVIS.Shared
{
    /// <summary>
    /// Network Related Helpers.
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Gets the IP address of the specified hostname.
        /// </summary>
        /// <returns>The internet protocol address.</returns>
        /// <param name="hostname">The provided hostname.</param>
        public static string GetIPAddress(string hostname)
        {
            if (IPAddress.TryParse(hostname, out IPAddress address))
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
                return "127.0.0.1";
            }

            return returnHost;
        }

        /// <summary>
        /// Validates that a provided port is valid and within acceptable ranges.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>A exception if there is one, otherwise null.</returns>
        public static Exception ValidatePort(int port)
        {
            if (port >= 0 && port <= 65535)
            {
                return null;
            }
            return new FormatException("A port must be greater than 0 and less than 65535");
        }

        /// <summary>
        /// Validates a provided host address.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>A exception if there is one, otherwise null.</returns>
        public static Exception ValidateHost(string host)
        {
            if (string.IsNullOrEmpty(host) || host.Trim() == "")
            {
                return new ArgumentNullException(nameof(host));
            }

            if (IPAddress.TryParse(host, out IPAddress address))
            {
                return null;
            }
            return new FormatException("The host format is invalid.");
        }
    }
}
