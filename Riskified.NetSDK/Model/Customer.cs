using System;
using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    [JsonObject("customer")]
    public class Customer
    {
        [JsonProperty(PropertyName = "created_at", Required = Required.Default)]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "last_name", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "note", Required = Required.Default)]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "orders_count", Required = Required.Default)]
        public int OrdersCount { get; set; }

        [JsonProperty(PropertyName = "verified_email",Required = Required.Default)]
        public bool VerifiedEmail { get; set; }
    }
}
