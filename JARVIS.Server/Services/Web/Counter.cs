using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Server.Services.Web
{
    [RestResource]
    public class Counter
    {
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/plus")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/plus/")]
        public IHttpContext Plus(IHttpContext context)
        {


            //Shared.Log.Message("DB", "Setting Server.Host: " + optionHost.Value());
            //DB.Connection.InsertOrReplace(new Tables.Settings()
            //{
            //    Name = "Server.Host",
            //    Value = optionHost.Value()
            //});


            //string[] parameters = Shared.Web.GetParameters(context.Request.RawUrl, context.Request.PathInfo);

            //// Build argument list
            //string args = "";
            //foreach (string s in parameters)
            //{
            //    args += s.Trim() + " ";
            //}
            //args.Trim();

            //Program.Socket.SendToAllSessions("Wirecast.Layers", args);

            //context.Response.SendResponse("OK");
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/minus")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/minus/")]
        public IHttpContext Minus(IHttpContext context)
        {
            return context;
        }

        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/set")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/set/")]
        public IHttpContext Set(IHttpContext context)
        {
            return context;
        }
    }
}