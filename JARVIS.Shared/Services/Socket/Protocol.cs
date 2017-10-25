using System;
using System.Collections.Generic;
using System.Text;
using SuperSocket.ProtoBase;

namespace JARVIS.Shared.Services.Socket
{
    public class Protocol : TerminatorReceiveFilter<StringPackageInfo>
    {
        public const string Terminator = "<EOL>";
        public const string Deliminator = "||";
        public string Content { get; set; }

        public Protocol() : base(Encoding.ASCII.GetBytes(Terminator))
        {
        }

        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            Content = bufferStream.ReadString((int)bufferStream.Length - Terminator.Length, Encoding.UTF8);

            string[] split = Content.Split(new string[] { Deliminator }, StringSplitOptions.RemoveEmptyEntries);

            List<string> parameters = new List<string>();
            for (int i = 1; i < split.Length; i += 2)
            {
                if ((i + 1) < split.Length)
                {
                    parameters.Add(split[i] + Deliminator + split[i + 1]);
                }
            }

            // Return no body
            return new StringPackageInfo(split[0].Trim(), null, parameters.ToArray());
        }

        public static Dictionary<string, string> GetStringDictionary(string[] parameters)
        {
            // Split out parameters
            Dictionary<string, string> returnParams = new Dictionary<string, string>();
            foreach (string s in parameters)
            {
                var param = s.Split(new string[] { Deliminator }, StringSplitOptions.RemoveEmptyEntries);
                returnParams.Add(param[0], param[1]);
            }
            return returnParams;
        }

        public static Dictionary<string, string> GetStringDictionary(string parameters)
        {
            Dictionary<string, string> returnParameters = new Dictionary<string, string>();

            string[] splitParameters = parameters.Split(new[] { Deliminator }, StringSplitOptions.None);

            for (int i = 0; i < splitParameters.Length; i++)
            {
                if ((i + 1) < splitParameters.Length)
                {
                    returnParameters.Add(splitParameters[i], splitParameters[i + 1]);
                }
            }

            return returnParameters;
        }

        public static string GetParameterString(Dictionary<string, string> parameters)
        {
            string returnString = "";

            foreach (string s in parameters.Keys)
            {
                returnString += s + Deliminator + parameters[s] + Deliminator;
            }

            if (returnString.EndsWith(Deliminator, StringComparison.InvariantCultureIgnoreCase))
            {
                returnString = returnString.Substring(0, returnString.Length - Deliminator.Length);
            }

            return returnString;
        }

        public static byte[] GetBytes(Commands.Types type, string body, Dictionary<string, string> parameters)
        {
            return Encoding.UTF8.GetBytes(type.GetSocketCommand() + Deliminator + GetParameterString(parameters));
        }
    }
}
