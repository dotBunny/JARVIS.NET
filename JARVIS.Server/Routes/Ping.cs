using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Server.Routes
{
    [RestResource]
    public class Ping
    {
		[RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/ping")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/ping/")]
		public IHttpContext Response(IHttpContext context)
		{
			context.Response.SendResponse("Pong");
			return context;
		}
    }
}