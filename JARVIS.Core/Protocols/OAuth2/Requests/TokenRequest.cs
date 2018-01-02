using System;
using System.Collections.Generic;
using JARVIS.Core.Protocols.OAuth2.Responses;
using Newtonsoft.Json;

namespace JARVIS.Core.Protocols.OAuth2.Requests
{
    public class TokenRequest
    {
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
                "&code=" + Uri.EscapeDataString(Code) + 
                "&redirect_uri=" + Uri.EscapeDataString(RedirectURI) + 
                "&state=" + Uri.EscapeDataString(State);
        }
    }
}
