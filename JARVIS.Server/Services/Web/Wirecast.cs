using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Server.Services.Web
{
	[RestResource]
	public class Wirecast
	{
		[RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers/")]
		public IHttpContext Layers(IHttpContext context)
		{

            string[] parameters = Shared.Web.GetParameters(context.Request.RawUrl, context.Request.PathInfo);

            // Build argument list
            string args = "";
            foreach (string s in parameters)
            {
                args += s.Trim() + " ";
            }
            args.Trim();

            Program.Socket.SendToAllSessions("Wirecast.Layers", args);

            context.Response.SendResponse("OK");
			return context;
		}
	}
}