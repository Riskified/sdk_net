using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Riskified.SDK.Notifications
{
    public class Notification
    {
        /// <summary>
        /// A unique order ID received from the Riskified server for later submittion or notification regarding that specific order
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int OrderId { get; set; }

        /// <summary>
        /// Textual value 
        /// </summary>
        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public OrderStatus Status { get; set; }

        /// <summary>
        /// A human readable description of the status received
        /// </summary>
        [JsonProperty(PropertyName = "description", Required = Required.Default, NullValueHandling=NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public string Description { get; set; }

        [JsonConstructor]
        public Notification(int orderId, OrderStatus status, string description)
        {
            OrderId = orderId;
            Status = status;
            Description = description;
        }

        public Notification()
        {

        }
    }
}
