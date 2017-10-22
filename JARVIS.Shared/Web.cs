using System;
using System.Collections.Generic;
namespace JARVIS.Shared
{
    public static class Web
    {
        private static char[] ParameterStart = { '?' };
        private static char[] ParameterDelimiter = { ',' };

        public static Dictionary<string, string> GetStringDictionary(System.Collections.Specialized.NameValueCollection parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            foreach(string s in parameters.AllKeys)
            {
                returnParameters.Add(s, parameters[s]);
            }

            return returnParameters;
        }
    }
}