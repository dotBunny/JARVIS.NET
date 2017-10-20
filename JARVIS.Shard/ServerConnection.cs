using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;

namespace JARVIS.Shard
{
    public class ServerConnection
    {
        public string Host = "localhost";
        public int Port = 8081;
        public bool IsConnected = false;

        private static EasyClient Connection;


        public ServerConnection()
        {
            // Initialize Shard Connection
            Connection = new EasyClient();


            Connection.Initialize(new SocketFilter(), (request) => {

                // Handle Requests

                // handle the received request
                Console.WriteLine(request.Key);

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
            }
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


        public class SocketFilter : TerminatorReceiveFilter<StringPackageInfo>
        {
            public SocketFilter() : base(Encoding.ASCII.GetBytes(Shared.Net.SocketTerminator))
            {
            }

            public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
            {
                throw new NotImplementedException();
            }
        }
    }
}
