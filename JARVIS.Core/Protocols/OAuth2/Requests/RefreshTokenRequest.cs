using System;
using System.Collections.Generic;
using JARVIS.Core.Protocols.OAuth2.Responses;
using Newtonsoft.Json;

namespace JARVIS.Core.Protocols.OAuth2.Requests
{
    public class RefreshTokenRequest
    {
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
            GrantType = "refresh_token";
        }

        public RefreshTokenRequest(string refreshToken, string state)
        {
            GrantType = "refresh_token";
            RefreshToken = refreshToken;
            State = state;
        }

        public TokenResponse GetResponse(string endpoint)
        {
            var json = Shared.Web.PostJSON(endpoint, ToFormData(), Headers);
            return JsonConvert.DeserializeObject<TokenResponse>(json);
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToFormData()
        {
            return "grant_type=" + Uri.EscapeDataString(GrantType) +
                "&refresh_token=" + Uri.EscapeDataString(RefreshToken) +
                "&state=" + Uri.EscapeDataString(State);
        }
    }
}

