using Newtonsoft.Json;

namespace SmartPi.Interface
{
    public class OAuthClient
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
    }
}