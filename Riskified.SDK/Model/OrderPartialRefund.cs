using Newtonsoft.Json;
using Riskified.SDK.Model.RefundElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderPartialRefund : AbstractOrder
    {
        [JsonProperty(PropertyName = "refunds", Required = Required.Always)]
        public RefundDetails[] Refunds { get; set; }

        public OrderPartialRefund(int merchantOrderId,RefundDetails[] partialRefunds) : base(merchantOrderId)
        {
            InputValidators.ValidateObjectNotNull(partialRefunds,"Refunds");
            Refunds = partialRefunds;
        }
    }
}
