﻿namespace JARVIS.Shard.Services.Socket
{
    public static class CommandFactory
    {
        public static ISocketCommand CreateCommand(Shared.Protocol.Instruction.OpCode commandType)
        {
            switch(commandType)
            {
                case Shared.Protocol.Instruction.OpCode.AUTH:
                    return new Commands.Auth();
                case Shared.Protocol.Instruction.OpCode.PONG:
                    return new Commands.Pong();
                case Shared.Protocol.Instruction.OpCode.COUNTER_SET:
                    return new Commands.Counter();
                case Shared.Protocol.Instruction.OpCode.INFO:
                    return new Commands.Info();
                case Shared.Protocol.Instruction.OpCode.WIRECAST_LAYERS:
                    return new Commands.Wirecast();
            }
            return new Commands.Default();

        }
    }
}
