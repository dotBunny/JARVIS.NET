using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.WebAPI.Requests
{
    public class RefreshTokenRequest
    {
        public static string Endpoint = "/api/token";

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

