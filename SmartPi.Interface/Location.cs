using Newtonsoft.Json;

namespace SmartPi.Interface
{
    public class Location
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}