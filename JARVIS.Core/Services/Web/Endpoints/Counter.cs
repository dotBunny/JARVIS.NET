using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Server.Attributes;
using Grapevine.Shared;

namespace JARVIS.Core.Services.Web.Endpoints
{
    /// <summary>
    /// The Counter set of EndPoints
    /// </summary>
    [RestResource]
    public class Counter
    {
        /// <summary>
        /// Counter.Plus EndPoint
        /// </summary>
        /// <returns>The updated request context.</returns>
        /// <param name="context">The request context.</param>
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/plus")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/plus/")]
        public IHttpContext Plus(IHttpContext context)
        {
            string[] parameters = Shared.Web.GetParameters(context.Request.RawUrl, context.Request.PathInfo);
            string counterName = "";
            int previousValue = 0;

            if ( parameters.Length > 0 ) {

                counterName = parameters[0].Trim();

                List<Database.Tables.Counters> values = Server.Database.Connection.Query<Database.Tables.Counters>("SELECT * FROM \"" + Database.Tables.Counters.GetTableName() + "\" WHERE \"Name\" = \"" + counterName + "\"");
                if ( values.Count > 0 )
                {
                    previousValue = values[0].Value;
                }


                // Increment Value
                previousValue++;

                Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = counterName,
                    Value = previousValue
                });

                Shared.Log.Message("DB", "Incremented Counter: " + counterName);
                Server.Socket.SendToAllSessions("Counter.Plus", counterName + Shared.Net.SocketDeliminator + previousValue);

                context.Response.SendResponse(Shared.Net.WebSuccessCode);
            } else {
                context.Response.SendResponse(Shared.Net.WebFailCode);
            }

            return context;
        }

        /// <summary>
        /// Counter.Minus EndPoint
        /// </summary>
        /// <returns>The updated request context.</returns>
        /// <param name="context">The request context.</param>
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/minus")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/minus/")]
        public IHttpContext Minus(IHttpContext context)
        {
            string[] parameters = Shared.Web.GetParameters(context.Request.RawUrl, context.Request.PathInfo);
            string counterName = "";
            int previousValue = 0;

            if (parameters.Length > 0)
            {

                counterName = parameters[0].Trim();

                List<Database.Tables.Counters> values = Server.Database.Connection.Query<Database.Tables.Counters>("SELECT * FROM '" + Database.Tables.Counters.GetTableName() + "' WHERE \"Name\" = \"" + counterName + "\"");
                if (values.Count > 0)
                {
                    previousValue = values[0].Value;
                }

                // Decrease Value
                previousValue--;
                if (previousValue < 0) previousValue = 0;

                Core.Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = counterName,
                    Value = previousValue
                });

                Shared.Log.Message("DB", "Decremented Counter: " + counterName);
                Core.Server.Socket.SendToAllSessions("Counter.Minus", counterName + Shared.Net.SocketDeliminator + previousValue);

                context.Response.SendResponse(Shared.Net.WebSuccessCode);
            }
            else
            {
                context.Response.SendResponse(Shared.Net.WebFailCode);
            }

            return context;
        }

        /// <summary>
        /// Counter.Set EndPoint
        /// </summary>
        /// <returns>The updated request context.</returns>
        /// <param name="context">The request context.</param>
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/set")]
        [RestRoute(HttpMethod = HttpMethod.GET, PathInfo = "/counter/set/")]
        public IHttpContext Set(IHttpContext context)
        {
            string[] parameters = Shared.Web.GetParameters(context.Request.RawUrl, context.Request.PathInfo);
            string counterName = "";
            int setValue = 0;

            if (parameters.Length > 1)
            {

                counterName = parameters[0].Trim();
                int.TryParse(parameters[1].Trim(), out setValue);

                Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = counterName,
                    Value = setValue
                });

                Shared.Log.Message("DB", "Set Counter: " + counterName + " as " + setValue);
                Core.Server.Socket.SendToAllSessions("Counter.Set", counterName + Shared.Net.SocketDeliminator + setValue);

                context.Response.SendResponse(Shared.Net.WebSuccessCode);
            }
            else
            {
                context.Response.SendResponse(Shared.Net.WebFailCode);
            }

            return context;
        }
    }
}