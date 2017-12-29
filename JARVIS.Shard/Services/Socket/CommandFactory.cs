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
                case Shared.Protocol.Instruction.OpCode.PONG:
                    return new Commands.Pong();
                case Shared.Protocol.Instruction.OpCode.FAIL:
                    return new Commands.Fail();
                case Shared.Protocol.Instruction.OpCode.COUNTER_SET:
                case Shared.Protocol.Instruction.OpCode.COUNTER_PLUS:
                case Shared.Protocol.Instruction.OpCode.COUNTER_MINUS:    
                    return new Commands.Counter();
                case Shared.Protocol.Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}
