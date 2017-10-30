namespace JARVIS.Shard.Services.Socket
{
    public static class CommandFactory
    {
        public static ISocketCommand CreateCommand(Shared.Services.Socket.Commands.Types commandType)
        {
            switch(commandType)
            {
                case Shared.Services.Socket.Commands.Types.AUTH:
                    return new Commands.Auth();
                case Shared.Services.Socket.Commands.Types.PONG:
                    return new Commands.Pong();
                case Shared.Services.Socket.Commands.Types.COUNTER_SET:
                    return new Commands.Counter();
                case Shared.Services.Socket.Commands.Types.INFO:
                    return new Commands.Info();
                case Shared.Services.Socket.Commands.Types.WIRECAST_LAYERS:
                    return new Commands.Wirecast();
            }
            return new Commands.Default();

        }
    }
}
