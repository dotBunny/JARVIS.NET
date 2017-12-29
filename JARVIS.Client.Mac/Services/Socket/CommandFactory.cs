namespace JARVIS.Client.Mac.Services.Socket
{
    public static class CommandFactory
    {
        public static Shared.Services.Socket.ISocketCommand CreateCommand(Shared.Protocol.Instruction.OpCode commandType, SocketClient client)
        {
            switch (commandType)
            {
                case Shared.Protocol.Instruction.OpCode.AUTH:
                    return new Commands.Auth(client);
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
                case Shared.Protocol.Instruction.OpCode.WIRECAST_LAYERS:
                    return new Commands.Wirecast();
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}