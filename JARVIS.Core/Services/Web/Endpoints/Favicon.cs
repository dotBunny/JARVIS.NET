using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
    [RestResource]
    public class FavoriteIcon
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/favicon.ico")]
        public IHttpContext Response(IHttpContext context)
        {
            context.Response.SendResponse(Shared.Resources.GetBytes("Resources.Icons.jarvis.ico"));
            return context;
        }
    }
}