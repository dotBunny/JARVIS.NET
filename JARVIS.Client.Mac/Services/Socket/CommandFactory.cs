using JARVIS.Shared.Services.Socket;

namespace JARVIS.Client.Mac.Services.Socket
{
    public class CommandFactory : ICommandFactory
    {
        ISocketClient Client;
        public CommandFactory(ISocketClient client)
        {
            Client = client;   
        }

        public ISocketCommand CreateCommand(Shared.Protocol.Instruction.OpCode commandType)
        {
            switch (commandType)
            {
                case Shared.Protocol.Instruction.OpCode.AUTH:
                    return new Commands.Auth(Client);
                case Shared.Protocol.Instruction.OpCode.PING:
                    return new Commands.Ping(Client);
                case Shared.Protocol.Instruction.OpCode.LOGIN_FAIL:
                    return new Commands.LoginFail();
                case Shared.Protocol.Instruction.OpCode.TEXT_FILE:
                    return new Client.Services.Socket.Commands.TextFile();
                case Shared.Protocol.Instruction.OpCode.BINARY_FILE:
                    return new Client.Services.Socket.Commands.BinaryFile();
                case Shared.Protocol.Instruction.OpCode.OAUTH_REQUEST:
                    return new Client.Services.Socket.Commands.OAuthRequest();
                case Shared.Protocol.Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
                case Shared.Protocol.Instruction.OpCode.WIRECAST_LAYERS:
                    return new Commands.WirecastLayers();
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}