using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Commands
{
    public static class Counter
    {
        public static void Set(Dictionary<string, string> parameters)
        {
            if ( parameters.ContainsKey("name") && parameters.ContainsKey("UPDATED_VALUE"))
            {
                Shared.Log.Message("counter", "Setting " + parameters["name"] + " => " + parameters["UPDATED_VALUE"]);
                Shared.IO.WriteContents(System.IO.Path.Combine(Program.OutputPath, parameters["name"] + ".txt"), parameters["UPDATED_VALUE"]);      
            }
        }
    }
}
