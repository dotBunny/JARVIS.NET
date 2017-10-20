using System;
namespace JARVIS.Shard.Commands
{
    public static class Info
    {
        public static void Command(string arguments)
        {
            Shared.Log.Message("info", arguments);
        }
    }
}
