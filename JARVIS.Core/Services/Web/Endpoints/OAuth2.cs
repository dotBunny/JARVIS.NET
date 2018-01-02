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
            string state = context.Request.QueryString.GetValue<string>("state", string.Empty);
            if ( Server.Web.CallbackListeners.ContainsKey(state) ) {

                // Handle callback
                Server.Web.CallbackListeners[state].Callback(context.Request);

                // Only get one shot
                Server.Web.CallbackListeners.Remove(state);
            }

            context.Response.SendResponse("Processed! You may close this window.");
            return context;
        }
    }
}