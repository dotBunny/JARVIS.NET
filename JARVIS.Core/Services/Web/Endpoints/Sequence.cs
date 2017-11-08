using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
    [RestResource]
    public class Sequence
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/sequence")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/sequence/")]
        public IHttpContext Command(IHttpContext context)
        {
            // Get parameters
            Dictionary<string, string> parameters = Shared.Web.GetStringDictionary(context.Request.QueryString);

            // Special sequence of events firing
            // ?0=counter.plus||name||saves&1=wait||seconds||1&2=wirecast.layer||0||clear||1||main
            // ?0=info||message||Sending%201%20Message&1=wait||seconds||5&2=info||message||Sending%201%20Message

            // Fire and forget sequence
            Task.Run(() => HandleSequence(parameters));

            context.Response.SendResponse(Shared.Web.SuccessCode);
            return context;
        }

        static void HandleSequence(Dictionary<string,string> commands)
        {
            string currentCommand;
            Dictionary<string, string> currentParameters = new Dictionary<string, string>();
            string urlParameters;

            // Iterate over commands
            foreach(string key in commands.Keys)
            {
                currentCommand = commands[key].Split(new[] { Shared.Web.Deliminator }, StringSplitOptions.None)[0];

                currentParameters.Clear();

                currentParameters = Shared.Web.GetStringDictionaryEscaped(
                    commands[key].Substring(
                        currentCommand.Length + 
                        Shared.Web.Deliminator.Length));

                // Create parameters for individual command to be issued
                urlParameters = "";
                foreach (KeyValuePair<string, string> param in currentParameters)
                {
                    urlParameters += "&" + Uri.EscapeDataString(param.Key) + "=" + Uri.EscapeDataString(param.Value);
                }
                urlParameters = urlParameters.TrimStart(new char[] { '&'} );



                switch (currentCommand.ToUpper()) {
                    case "SEQUENCE":
                        // No infinite loop K thnx
                        break;
                    case "WAIT":
                        if (currentParameters.ContainsKey("seconds"))
                        {
                            Task.Delay(int.Parse(currentParameters["seconds"]) * 1000);
                        }
                        break;
                    default:

                        string touchURL = "http://" + Server.Config.Host + ":" +
                            Server.Config.WebPort.ToString() +
                            "/" + currentCommand.ToLower().Replace(".", "/") + "/?" +
                      
                                                            urlParameters;

                        Shared.Log.Message("web", "Sequence -> " + touchURL);
                        Shared.Web.Touch(touchURL);
                        
                        break;
                }
            }
        } 
    }
}