using System;
using System.Collections.Generic;
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


            Connection.Initialize(new SocketFilter(), (request) =>
            {
                Shared.Log.Message("socket", "Request Received ->" + request.Key.ToUpper());

                // Split out parameters
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                foreach(string s in request.Parameters)
                {
                    var param = s.Split(new string[] { Shared.Net.SocketDeliminator }, StringSplitOptions.RemoveEmptyEntries);
                    parameters.Add(param[0], param[1]);
                }

                // Send to commands
                switch(request.Key.ToUpper()) {

                    case "INFO":
                        Commands.Info.Command(parameters);
                        break;
                    case "WIRECAST.LAYERS":
                        if (Program.HasWirecastSupport)
                        {
                            Commands.Wirecast.Layers(parameters);
                        }
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
                Program.Shutdown(1);
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

                string[] split = package.Split(new string[] { Shared.Net.SocketDeliminator }, StringSplitOptions.RemoveEmptyEntries);

                List<string> parameters = new List<string>();
                for (int i = 1; i < split.Length; i+=2) {
                    if ((i + 1) < split.Length)
                    {
                        parameters.Add(split[i] + Shared.Net.SocketDeliminator + split[i + 1]);
                    }
                }

                // Return no body
                return new StringPackageInfo(split[0].Trim(), null, parameters.ToArray());
            }
        }
    }
}
