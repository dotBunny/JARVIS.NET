using System;
namespace JARVIS.Shard.Commands
{
    public static class Wirecast
    {
        public static void Layers(string arguments) 
        {
            Shared.Log.Message("Wire", "Changing to " + arguments);
            Shared.Platform.Run(System.IO.Path.Combine(Shared.Platform.GetBaseDirectory(), "Resources", "macOS", "Wirecast.appleScript"), arguments, true);
        }
    }
}
