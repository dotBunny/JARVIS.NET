using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using JARVIS.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
	[RestResource]
	public class Wirecast
	{
		[RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers/")]
		public IHttpContext Layers(IHttpContext context)
		{
            // ?layer1=name&layer2=name&layer3=name&layer4=name&layer5=name

            // Send command via socket
            Server.Socket.SendToAllSessions(
                "Wirecast.Layers", 
                Net.GetParameterString(
                    Shared.Web.GetStringDictionary(
                        context.Request.QueryString)));

            context.Response.SendResponse(Net.WebSuccessCode);
			return context;
		}
	}
}