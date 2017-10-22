using System;
using System.Collections.Generic;

namespace JARVIS.Shard.Commands
{
    public static class Info
    {
        public static void Command(Dictionary<string, string> parameters)
        {
            foreach(string s in parameters.Keys) 
            {
                Shared.Log.Message("info", s + " => " + parameters[s]);    
            }
        }
    }
}
