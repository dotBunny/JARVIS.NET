using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.JSON
{
    public class TokenResponse
    {
        public static string Endpoint = "/api/token/";
       
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresInSeconds { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("error")]
        public string ErrorCode { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}