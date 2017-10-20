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
            string args = "";

            foreach(string s in parameters) {
                args += " " + s;
            }

            Shared.Platform.Run(System.IO.Path.Combine(Shared.Platform.GetBaseDirectory(), "Resources", "macOS", "Wirecast.appleScript"), args, true);

            context.Response.SendResponse("Changed.");
			return context;
		}
	}
}