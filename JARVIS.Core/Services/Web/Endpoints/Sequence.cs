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

            // Special where we look at 
            // ?command1=wirecast.layer&args=&command2=counter.plus&args2=crashes

            context.Response.SendResponse(Shared.Net.WebSuccessCode);
            return context;
        }
    }
}