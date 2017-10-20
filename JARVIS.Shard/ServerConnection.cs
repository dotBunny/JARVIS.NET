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

                Shared.Log.Message("socket", "Request Received ->" + request.Key.ToUpper());
                switch(request.Key.ToUpper()) {

                    case "INFO":
                        Commands.Info.Command(request.Body);
                        break;
                    case "WIRECAST.LAYERS":
                        Commands.Wirecast.Layers(request.Body);
                        break;
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
                string package = bufferStream.ReadString((int)bufferStream.Length - Shared.Net.SocketTerminator.Length, Encoding.UTF8);

                string[] split = package.Split(new string[] { Shared.Net.SocketDeliminator }, StringSplitOptions.None);

                return new StringPackageInfo(split[0].Trim(), split[1].Trim(), null);
            }
        }
    }
}
