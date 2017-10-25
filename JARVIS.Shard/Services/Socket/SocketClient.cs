using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using JARVIS.Shared.Services.Socket;
using SuperSocket.ProtoBase;

namespace JARVIS.Shard.Services.Socket
{
    public class SocketClient 
    {
        public string Host = "localhost";
        public int Port = 8081;

        public bool IsConnected {
            get { return Connection.IsConnected;  }
        }

        EasyClient<StringPackageInfo> Connection = new EasyClient<StringPackageInfo>();

        public SocketClient()
        {
            // Initialize Protocol
            Connection.Initialize(new Protocol());

            // Setup event handlers
            Connection.Connected += Connection_Connected;
            Connection.Closed += Connection_Closed;
            Connection.Error += Connection_Error;
            Connection.NewPackageReceived += Connection_NewPackageReceived;
        }

        public async Task Start()
        {
            Host = Shared.Net.GetIPAddress(Host);
                
            var connected = await Connection.ConnectAsync(new IPEndPoint(IPAddress.Parse(Host), Port));
            if (!connected)
            {
                Shared.Log.Message("socket", "Unable to connect to " + Host + ":" + Port.ToString());
            }
        }

        public void Stop()
        {
            // Disconnect
            Connection.Close();
        }

        public void Send(Shared.Services.Socket.Commands.Types type, string body, Dictionary<string, string> parameters)
        {
            Shared.Log.Message("socket", "Sending " + type.ToString() + " to " + Host + ":" + Port.ToString());
            Connection.Send(new ArraySegment<byte>(Protocol.GetBytes(type, body, parameters)));
        }

        void Connection_Connected(object sender, EventArgs e)
        {
            Shared.Log.Message("socket", "Connected to " + Shared.Net.GetIPAddress(Host) + ":" + Port.ToString());

            //Send(Shared.Services.Socket.Commands.Types.AUTH, string.Empty, new Dictionary<string, string>() { });
        }

        void Connection_Closed(object sender, EventArgs e)
        {
            Shared.Log.Message("socket", "Disconnected from " + Shared.Net.GetIPAddress(Host) + ":" + Port.ToString());

            // Attempt reconnect?

            //Program.Shutdown(1);
        }

        void Connection_Error(object sender, ErrorEventArgs e)
        {
            Shared.Log.Error("socket", "An error occured. " + e.Exception);
        }

        void Connection_NewPackageReceived(object sender, PackageEventArgs<StringPackageInfo> e)
        {
            Shared.Log.Message("socket", "Package Received -> " + e.Package.Key.ToUpper());

            // Factory Pattern
            ISocketCommand receivedCommand = CommandFactory.CreateCommand(e.Package.Key.ToUpper());

            // Move forward?
            if (receivedCommand.CanExecute())
            {
                receivedCommand.Execute(Protocol.GetStringDictionary(e.Package.Parameters));
            }
        }
    }
}
