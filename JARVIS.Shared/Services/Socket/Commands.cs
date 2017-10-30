using System;
namespace JARVIS.Shared.Services.Socket
{
    public static class Commands
    {
        public enum Types
        {
            DEFAULT,
            PING,
            PONG,
            AUTH,
            LOGIN,
            INFO,
            COUNTER_SET,
            COUNTER_PLUS,
            COUNTER_MINUS,
            WIRECAST_LAYERS
        }

        public static Types GetType(this string type)
        {
            // Sanitize Type
            type = type.Replace(".", "_").Replace("/", "_");

            Types returnType = Types.DEFAULT;
            if (Enum.TryParse<Types>(type, true, out returnType))
            {
                return returnType;
            }
            return Types.DEFAULT;
        }
    }
}
