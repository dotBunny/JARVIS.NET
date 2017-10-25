using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace JARVIS.Shard.Services.Socket
{
    public class SocketService
    {
        public string Host = "localhost";
        public int Port = 8081;
        public bool IsConnected = false;

        private static EasyClient Connection;

        public SocketService()
        {
            // Initialize Shard Connection
            Connection = new EasyClient();

            Connection.Initialize(new Shared.Services.Socket.SocketFilter(), (request) =>
            {
                Shared.Log.Message("socket", "Request Received -> " + request.Key.ToUpper());

                // Factory Pattern
                ICommand receivedCommand = CommandFactory.CreateCommand(request.Key.ToUpper());
                if (receivedCommand.CanExecute())
                {
                    receivedCommand.Execute(Shared.Services.Socket.SocketFilter.GetStringDictionary(request.Parameters));
                }
            });
        }

        public async void Start()
        {
            Task<bool> connectionResult = Connect();
            IsConnected = await connectionResult;

            if (IsConnected)
            {
                Shared.Log.Message("system", "Connected to server (" + Shared.Net.GetIPAddress(Host) + ":" + Port.ToString() + ").");
                Connection.Send(Encoding.ASCII.GetBytes("LOGIN shard"));
            }
            else
            {
                Shared.Log.Error("system", "Unable to server (" + Shared.Net.GetIPAddress(Host) + ":" + Port.ToString() + ").");
                Program.Shutdown(1);
            }
        }

        public void Send(Shared.Services.Socket.Commands.Types type, string body, Dictionary<string, string> parameters)
        {
            Send(Shared.Services.Socket.SocketFilter.GetSocketBytes(type, body, parameters));
        }

        public void Send(byte[] data)
        {
            Connection.Send(data);   
        }


        async Task<bool> Connect()
        {
            var connected = await Connection.ConnectAsync(new IPEndPoint(IPAddress.Parse(Shared.Net.GetIPAddress(Host)), Port));
            return connected;
        }

        public void Stop()
        {
            // Disconnect
            Connection.Close();
        }
    }
}
