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
                case Instruction.OpCode.AUTH_SPOTIFY:
                    return new Commands.AuthSpotify();
                case Instruction.OpCode.AUTH_DISCORD:
                    return new Commands.AuthDiscord();
                case Instruction.OpCode.AUTH_STREAMLABS:
                    return new Commands.AuthStreamlabs();
                case Instruction.OpCode.LOGIN:
                    return new Commands.Login();
                case Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
                case Instruction.OpCode.PING:
                    return new Commands.Ping();   

            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}
