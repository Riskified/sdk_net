using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderIdOnly : OrderBase
    {
        public OrderIdOnly(string merchantOrderId) : base(merchantOrderId)
        {
        }
    }
}
