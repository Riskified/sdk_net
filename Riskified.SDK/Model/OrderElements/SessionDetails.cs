using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Riskified.SDK.Model.OrderElements
{
    public class SessionDetails
    {
        public SessionDetails(DateTimeOffset? createdAt, string cartToken, string browserIp, SessionSource source)
        {
            CreatedAt = createdAt;
            CartToken = cartToken;
            BrowserIp = browserIp;
            Source = source;
        }

        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "cart_token")]
        public string CartToken { get; set; }

        [JsonProperty(PropertyName = "browser_ip")]
        public string BrowserIp { get; set; }

        [JsonProperty(PropertyName = "source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SessionSource Source { get; set; }

        [JsonProperty(PropertyName = "device_id")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "referring_site")]
        public string ReferringSite { get; set; }
    }
}
