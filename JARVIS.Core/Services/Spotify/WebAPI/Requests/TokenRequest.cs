using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.WebAPI.Requests
{
    public class TokenRequest
    {
        public static string Endpoint = "/api/token";

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectURI { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
