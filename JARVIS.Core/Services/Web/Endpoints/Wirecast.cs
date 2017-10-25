using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;
using JARVIS.Shared.Services.Socket;

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
            Server.Socket.SendToAllSessions(Commands.Types.WIRECAST_LAYERS, 
                                            string.Empty,
                                            Shared.Web.GetStringDictionary(
                                                context.Request.QueryString));

            context.Response.SendResponse(Shared.Web.SuccessCode);
			return context;
		}
	}
}