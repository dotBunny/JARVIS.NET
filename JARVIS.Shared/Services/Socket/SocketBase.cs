﻿using System;
using System.Net.Sockets;
using System.Text;

namespace JARVIS.Shared.Services.Socket
{
    public abstract class SocketBase
    {
        protected internal System.Net.Sockets.Socket socket = null;

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public Encoding Encoding { get; set; } = Encoding.Default;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Client"/> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public bool Connected => (socket != null && socket.Connected);

        /// <summary>
        /// Gets the type of the protocol.
        /// </summary>
        /// <value>
        /// The type of the protocol.
        /// </value>
        public ProtocolType ProtocolType => socket.ProtocolType;
        
        /// <summary>
        /// Gets or sets the on connected.
        /// </summary>
        /// <value>
        /// The on connected.
        /// </value>
        public Action<Sender> OnConnected { get; set; } = null;

        /// <summary>
        /// Gets or sets the on data.
        /// </summary>
        /// <value>
        /// The on data.
        /// </value>
        public Action<Sender, byte[]> OnData { get; set; } = null;

        /// <summary>
        /// Gets or sets the on exception.
        /// </summary>
        /// <value>
        /// The on exception.
        /// </value>
        public Action<Sender, Exception> OnException { get; set; } = null;

        /// <summary>
        /// Gets or sets the on closed.
        /// </summary>
        /// <value>
        /// The on closed.
        /// </value>
        public Action<Sender> OnClosed { get; set; } = null;

        /// <summary>
        /// Start Socket.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stop Socket.
        /// </summary>
        public abstract void Stop();
        
        /// <summary>
        /// Sets the host.
        /// </summary>
        /// <param name="host">The host.</param>
        protected void SetHost(string host)
        {
            Exception ex = Net.ValidateHost(host);
            if (ex == null)
            {
                Host = host;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets the port.
        /// </summary>
        /// <param name="port">The port.</param>
        protected void SetPort(int port)
        {
            Exception ex = Net.ValidatePort(port);
            if (ex == null)
            {
                Port = port;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks the host and port.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Host is incorrect
        /// or
        /// Port is incorrect
        /// </exception>
        protected void CheckHostAndPort()
        {
            Exception exception = Net.ValidateHost(Host);

            if (exception != null)
            {
                throw new Exception("Host is incorrect", exception);
            }

            exception = Net.ValidatePort(Port);

            if (exception != null)
            {
                throw new Exception("Port is incorrect", exception);
            }
        }
    }
}