using System;
using Newtonsoft.Json;

namespace JARVIS.Core.Services.Spotify.WebAPI.Responses
{
    public class ErrorMessage
    {
        [JsonProperty("status")]
        public string Code;

        [JsonProperty("message")]
        public string Description;
    }
}
