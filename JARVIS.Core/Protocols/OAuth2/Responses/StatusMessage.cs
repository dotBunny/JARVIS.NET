using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Protocols.OAuth2.Responses
{
    public class StatusMessage
    {
        [JsonProperty("status")]
        public string Code;

        [JsonProperty("message")]
        public string Description;

        [JsonProperty("success")]
        public bool Successful;
    }
}
