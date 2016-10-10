using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    internal class OrderWrapper<TOrder>
    {
        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public TOrder Order { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public string[] Warnings { get; set; }

        public OrderWrapper(TOrder order)
        {
            Order = order;
        }
    }
}
