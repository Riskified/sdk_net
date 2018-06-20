using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderIdOnly : AbstractOrder
    {
        public OrderIdOnly(string merchantOrderId) : base(merchantOrderId)
        {

        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }
    }
}
