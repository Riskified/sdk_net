using Newtonsoft.Json;
using Riskified.SDK.Orders.Model.RefundElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Orders.Model
{
    public class OrderRefund : AbstractOrder
    {
        [JsonProperty(PropertyName = "refunds", Required = Required.Always)]
        public RefundDetails[] Refunds { get; set; }

        public OrderRefund(int merchantOrderId,RefundDetails[] partialRefunds) : base(merchantOrderId)
        {
            InputValidators.ValidateObjectNotNull(partialRefunds,"Refunds");
            Refunds = partialRefunds;
        }
    }
}
