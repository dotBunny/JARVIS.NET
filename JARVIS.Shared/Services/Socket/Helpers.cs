using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace JARVIS.Shared.Services.Socket
{
    public static class Helpers
    {

        internal static byte[] Md5(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Trim() == "")
            {
                return null;
            }

            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            return result;
        }

        /// <summary>
        /// Sends data to a connected <see cref="Socket"/>.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this System.Net.Sockets.Socket socket, string data, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers);
        }

        /// <summary>
        /// Sends data to a connected <see cref="Socket" /> using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this System.Net.Sockets.Socket socket, string data, SocketFlags socketFlags, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, socketFlags);
        }

        /// <summary>
        ///  Sends the specified number of bytes of data to a connected <see cref="Socket" />, starting at the specified offset, and using the specified <see cref="SocketFlags" />.
        /// </summary>
        /// <param name="data">An string of type <see cref="string" /> that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags" /> values.</param>
        /// <param name="socketError">A <see cref="SocketError" /> object that stores the socket error.</param>
        /// <param name="encoding">The <see cref="Encoding"/> for data.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public static int Send(this System.Net.Sockets.Socket socket, string data, SocketFlags socketFlags, out SocketError socketError, Encoding encoding)
        {
            byte[] buffers = encoding.GetBytes(data);
            return socket.Send(buffers, 0, buffers.Length, socketFlags, out socketError);
        }

        /// <summary>
        /// Gets the host.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static string GetHost(this EndPoint point)
        {
            IPEndPoint ipoint = point as IPEndPoint;
            return ipoint.Address.ToString();
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static int GetPort(this EndPoint point)
        {
            IPEndPoint ipoint = point as IPEndPoint;
            return ipoint.Port;
        }
    }
}
