using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Orders.Model
{
    public abstract class AbstractOrder
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int? Id { get; set; }

        protected AbstractOrder(int merchantOrderId)
        {
            InputValidators.ValidatePositiveValue(merchantOrderId, "Merchant Order ID");
            Id = merchantOrderId;
        }
    }
}
