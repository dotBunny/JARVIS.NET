using System;
using RestSharp.Deserializers;

namespace JARVIS.Core.Services.Spotify.JSON
{
    public class TokenResponse
    {
        public static string Endpoint = "api/token";
       
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "token_type")]
        public string TokenType { get; set; }

        [DeserializeAs(Name = "scope")]
        public string Scope { get; set; }

        [DeserializeAs(Name = "expires_in")]
        public int ExpiresInSeconds { get; set; }

        [DeserializeAs(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DeserializeAs(Name = "error")]
        public string ErrorCode { get; set; }

        [DeserializeAs(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }
}