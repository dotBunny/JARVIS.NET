using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using JARVIS.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
    [RestResource]
    public class Info
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/info")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/info/")]
        public IHttpContext Response(IHttpContext context)
        {
            var parameters = Shared.Web.GetStringDictionary(context.Request.QueryString);
            foreach (string s in parameters.Keys)
            {
                Log.Message("info", s + " => " + parameters[s]);
            }

            // Send command via socket
            Server.Socket.SendToAllSessions("Info", Net.GetParameterString(parameters));

            context.Response.SendResponse(Net.WebSuccessCode);
            return context;
        }
    }
}