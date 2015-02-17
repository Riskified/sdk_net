using Newtonsoft.Json;
using System.Linq;
using Riskified.SDK.Model.RefundElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderPartialRefund : AbstractOrder
    {
        [JsonProperty(PropertyName = "refunds")]
        public PartialRefundDetails[] Refunds { get; set; }

        public OrderPartialRefund(int merchantOrderId,PartialRefundDetails[] partialRefunds) : base(merchantOrderId)
        {
            Refunds = partialRefunds;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
            InputValidators.ValidateObjectNotNull(Refunds, "Refunds");
            Refunds.ToList().ForEach(item => item.Validate(validationType));
        }
    }
}
