using JARVIS.Shared.Protocol;
using JARVIS.Shared.Services.Socket;

namespace JARVIS.Core.Services.Socket
{
    public class CommandFactory : ICommandFactory
    {
        public ISocketCommand CreateCommand(Instruction.OpCode commandType)
        {
            switch (commandType)
            {
                case Instruction.OpCode.AUTH:
                    return new Commands.Auth();
                case Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
                case Instruction.OpCode.PING:
                    return new Commands.Ping();   
                case Instruction.OpCode.SPOTIFY_REAUTH:
                    return new Commands.SpotifyReauth();
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}
