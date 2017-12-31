using System;
using System.Collections.Generic;
using JARVIS.Core.Services.Spotify.WebAPI.Responses;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.WebAPI.Requests
{
    public class RefreshTokenRequest
    {
        [JsonIgnore]
        public static string Endpoint = "https://accounts.spotify.com/api/token";

        [JsonIgnore]
        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }


        public RefreshTokenRequest()
        {
            Headers.Add("Content-Type", "application/json");
            GrantType = "refresh_token";
        }

        public RefreshTokenRequest(string refreshToken, string state)
        {
            Headers.Add("Content-Type", "application/json");
            GrantType = "refresh_token";
            RefreshToken = refreshToken;
            State = state;
        }

        public TokenResponse GetResponse()
        {
            var json = Shared.Web.POST(Endpoint, ToJSON(), Headers);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

