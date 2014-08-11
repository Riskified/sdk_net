using Newtonsoft.Json;

namespace Riskified.SDK.Orders.Model
{

    internal class GenericOrder
    {
        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public AbstractOrder Order { get; set; }

        public GenericOrder(AbstractOrder order)
        {
            Order = order;
        }
    }
}
