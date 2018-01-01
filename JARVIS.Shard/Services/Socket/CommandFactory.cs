using JARVIS.Shared.Services.Socket;

namespace JARVIS.Shard.Services.Socket
{
    public class CommandFactory : ICommandFactory
    {
        public CommandFactory()
        {
            
        }

        public ISocketCommand CreateCommand(Shared.Protocol.Instruction.OpCode commandType)
        {
            switch(commandType)
            {
                case Shared.Protocol.Instruction.OpCode.AUTH:
                    return new Commands.Auth();
                case Shared.Protocol.Instruction.OpCode.PING:
                    return new Commands.Ping();
                case Shared.Protocol.Instruction.OpCode.LOGIN_FAIL:
                    return new Commands.LoginFail();
                case Shared.Protocol.Instruction.OpCode.TEXT_FILE:    
                    return new Commands.TextFile();
                case Shared.Protocol.Instruction.OpCode.BINARY_FILE:
                    return new Commands.BinaryFile();
                case Shared.Protocol.Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}