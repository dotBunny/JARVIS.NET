using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
    [RestResource]
    public class OAuth2
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/callback")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/callback/")]
        public IHttpContext Response(IHttpContext context)
        {
            Shared.Log.Message("web", context.Request.QueryString.ToString());
            context.Response.SendResponse("Yay");
            return context;
        }
    }
}