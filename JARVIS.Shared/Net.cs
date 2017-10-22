using System;
using System.Collections.Generic;
using System.Net;

namespace JARVIS.Shared
{
    public static class Net
    {
        public const string SocketTerminator = "<EOL>";
        public const string SocketDeliminator = "||";
        public const string WebSuccessCode = "OK";
        public const string WebFailCode = "FAIL";

        public static string GetIPAddress(string hostname)
        {
            // Resolve hostname into IP of not IP
            IPHostEntry host = Dns.GetHostEntry(hostname);
            return host.AddressList[0].ToString();
        }

        public static string GetParameterString(Dictionary<string, string> parameters)
        {
            string returnString = "";

            foreach(string s in parameters.Keys)
            {
                returnString += s + SocketDeliminator + parameters[s] + SocketDeliminator;
            }

            if (returnString.EndsWith(SocketDeliminator, StringComparison.InvariantCultureIgnoreCase))
            {
                returnString = returnString.Substring(0, returnString.Length - SocketDeliminator.Length);
            }
         
            return returnString;
        }

        public static Dictionary<string, string> GetStringDictionary(string parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            string[] splitParameters = parameters.Split(new[] { SocketDeliminator }, StringSplitOptions.None);

            for (int i = 0; i < splitParameters.Length; i++)
            {
                if ((i + 1) < splitParameters.Length)
                {
                    returnParameters.Add(splitParameters[i], splitParameters[i + 1]);
                }
            }

            return returnParameters;
        }

    }
}
