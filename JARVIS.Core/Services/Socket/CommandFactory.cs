namespace JARVIS.Core.Services.Socket
{
    public static class CommandFactory
    {
        public static Shared.Services.Socket.ISocketCommand CreateCommand(Shared.Protocol.Instruction.OpCode commandType)
        {
            switch (commandType)
            {
                case Shared.Protocol.Instruction.OpCode.AUTH:
                    return new Commands.Auth();
                case Shared.Protocol.Instruction.OpCode.INFO:
                    return new Shared.Services.Socket.Commands.Info();
                case Shared.Protocol.Instruction.OpCode.PING:
                    return new Commands.Ping();                
            }
            return new Shared.Services.Socket.Commands.Default();

        }
    }
}
