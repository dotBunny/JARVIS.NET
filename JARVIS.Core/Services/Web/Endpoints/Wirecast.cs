using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
	[RestResource]
	public class Wirecast
	{
        public const string ScopeWirecastLayer = "wirecast-layers";

		[RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/wirecast/layers/")]
		public IHttpContext Layers(IHttpContext context)
		{
            

          //      .Request.RemoteEndPoint.Address)
            // ?L1=name&layer2=name&3=name&layer4=name&layer5=name

            // Send command via socket
            Server.Socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.WIRECAST_LAYERS, 
                                            Shared.Web.GetStringDictionary(context.Request.QueryString), 
                                            true,
                                            ScopeWirecastLayer);

            context.Response.SendResponse(Shared.Web.SuccessCode);
			return context;
		}
	}
}