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

            Dictionary<string, string> parameters = Shared.Web.GetStringDictionary(context.Request.QueryString);
            int previousValue = 0;

            if (parameters.Count > 0)
            {

                // Get previous value
                List<Database.Tables.Counters> values = Server.Database.Connection.Query<Database.Tables.Counters>("SELECT * FROM \"" + Database.Tables.Counters.GetTableName() + "\" WHERE \"Name\" = \"" + parameters["name"] + "\"");
                if (values.Count > 0)
                {
                    previousValue = values[0].Value;
                }


                if (parameters.ContainsKey("value"))
                {
                    // Increment Value
                    previousValue += int.Parse(parameters["value"]);
                }
                else
                {
                    // Increment Value
                    previousValue++;
                }

                Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = parameters["name"],
                    Value = previousValue
                });

                Shared.Log.Message("DB", "Incremented Counter: " + parameters["name"]);

                // Store value
                parameters["UPDATED_VALUE"] = previousValue.ToString();

                Server.Socket.SendToAllSessions(
                    Shared.Services.Socket.Commands.Types.COUNTER_PLUS, string.Empty,
                    parameters);
                
                context.Response.SendResponse(Shared.Web.SuccessCode);
            } else {
                context.Response.SendResponse(Shared.Web.FailCode);
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
            Dictionary<string, string> parameters = Shared.Web.GetStringDictionary(context.Request.QueryString);
            int previousValue = 0;

            if (parameters.Count > 0)
            {

                // Get previous value
                List<Database.Tables.Counters> values = Server.Database.Connection.Query<Database.Tables.Counters>("SELECT * FROM \"" + Database.Tables.Counters.GetTableName() + "\" WHERE \"Name\" = \"" + parameters["name"] + "\"");
                if (values.Count > 0)
                {
                    previousValue = values[0].Value;
                }

                // Decrease Value
                if (parameters.ContainsKey("value"))
                {
                    previousValue -= int.Parse(parameters["value"]);
                }
                else
                {
                    previousValue--;
                }
                if (previousValue < 0) previousValue = 0;

                Core.Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = parameters["name"],
                    Value = previousValue
                });

                Shared.Log.Message("DB", "Decremented Counter: " + parameters["name"]);

                // Store value
                parameters["UPDATED_VALUE"] = previousValue.ToString();

                Server.Socket.SendToAllSessions(
                    Shared.Services.Socket.Commands.Types.COUNTER_MINUS, string.Empty,
                    parameters);

                context.Response.SendResponse(Shared.Web.SuccessCode);
            }
            else
            {
                context.Response.SendResponse(Shared.Web.FailCode);
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
            Dictionary<string, string> parameters = Shared.Web.GetStringDictionary(context.Request.QueryString);
           
            int setValue = 0;

            if (parameters.ContainsKey("name") && parameters.ContainsKey("value"))
            {
                int.TryParse(parameters["value"].Trim(), out setValue);

                Server.Database.Connection.InsertOrReplace(new Database.Tables.Counters()
                {
                    Name = parameters["name"],
                    Value = setValue
                });

                Shared.Log.Message("DB", "Set Counter: " + parameters["name"] + " as " + setValue);
               
                // Store value
                parameters["UPDATED_VALUE"] = setValue.ToString();

                Server.Socket.SendToAllSessions(
                    Shared.Services.Socket.Commands.Types.COUNTER_SET, string.Empty,
                    parameters);

                context.Response.SendResponse(Shared.Web.SuccessCode);
            }
            else
            {
                context.Response.SendResponse(Shared.Web.FailCode);
            }
            return context;
        }
    }
}