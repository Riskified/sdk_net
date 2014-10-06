using Newtonsoft.Json;
using System.Linq;
using Riskified.SDK.Model.RefundElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderPartialRefund : AbstractOrder
    {
        [JsonProperty(PropertyName = "refunds", Required = Required.Always)]
        public PartialRefundDetails[] Refunds { get; set; }

        public OrderPartialRefund(int merchantOrderId,PartialRefundDetails[] partialRefunds) : base(merchantOrderId)
        {
            Refunds = partialRefunds;
        }

        public override void Validate(bool isWeak)
        {
            base.Validate(isWeak);
            InputValidators.ValidateObjectNotNull(Refunds, "Refunds");
            Refunds.ToList().ForEach(item => item.Validate(isWeak));
        }
    }
}
