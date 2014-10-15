using Newtonsoft.Json;

namespace Riskified.SDK.Model
{
    public class OrderNotification
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

    }
}
