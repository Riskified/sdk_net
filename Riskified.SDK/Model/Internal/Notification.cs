using Newtonsoft.Json;
using System;

namespace Riskified.SDK.Model.Internal
{
    internal class Notification
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Default)]
        public DateTime? UpdatedAt { get; set; } 

    }
}
