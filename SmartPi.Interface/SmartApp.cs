﻿using Newtonsoft.Json;

namespace SmartPi.Interface
{
    internal class SmartApp
    {
        [JsonProperty("oauthClient")]
        public string OAuthClient { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("base_url")]
        public string BaseUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        //   {
        //    "oauthClient": {
        //        "clientId": "CLIENT-ID"
        //    },
        //    "location": {
        //        "id": ID,
        //        "name": "LOCATION-NAME"
        //    }
        //    "uri": "BASE-URL/api/smartapps/installations/INSTALLATION-ID",
        //    "base_url": "BASE-URL",
        //    "url": "/api/smartapps/installations/INSTALLATION-ID"
        //}
    }
}
