using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Protocols.OAuth2.Responses
{
    public class ErrorMessage
    {
        [JsonProperty("status")]
        public string Code;

        [JsonProperty("message")]
        public string Description;
    }
}
