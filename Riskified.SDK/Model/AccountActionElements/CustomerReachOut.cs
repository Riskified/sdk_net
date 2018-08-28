using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class CustomerReachOut : AbstractAccountAction
    {
        public CustomerReachOut(string customerId, ContactMethod contactMethod) :
        base(customerId)
        {
            ContactMethod = contactMethod;
        }

        [JsonProperty(PropertyName = "order_id")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "contact_method")]
        public ContactMethod ContactMethod { get; set; }
    }
}
