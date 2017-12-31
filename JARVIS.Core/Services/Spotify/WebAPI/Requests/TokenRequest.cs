using System;
using System.Collections.Generic;
using JARVIS.Core.Services.Spotify.WebAPI.Responses;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.WebAPI.Requests
{
    public class TokenRequest
    {
        [JsonIgnore]
        public static string Endpoint = "https://accounts.spotify.com/api/token";

        [JsonIgnore]
        public Dictionary<string, string> Headers = new Dictionary<string, string>();


        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectURI { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }


        public TokenRequest()
        {
            GrantType = "authorization_code";
        }

        public TokenRequest(string code, string redirectURI, string state)
        {
            GrantType = "authorization_code";
            Code = code;
            RedirectURI = redirectURI;
            State = state;
        }

        public TokenResponse GetResponse()
        {
            var json = Shared.Web.PostJSON(Endpoint, ToJSON(), Headers);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
