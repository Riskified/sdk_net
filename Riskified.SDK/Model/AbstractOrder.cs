using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public abstract class AbstractOrder
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        protected AbstractOrder(int merchantOrderId)
        {
            InputValidators.ValidatePositiveValue(merchantOrderId, "Merchant Order ID");
            Id = merchantOrderId.ToString();
        }

        protected AbstractOrder(string merchantOrderId)
        {
            InputValidators.ValidateValuedString(merchantOrderId, "Merchant Order ID");
            Id = merchantOrderId;
        }
    }
}
