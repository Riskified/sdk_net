using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class ResetPasswordRequest : AbstractAccountAction
    {
        public ResetPasswordRequest(string customerId, ClientDetails clientDetails, SessionDetails sessionDetails) :
        base(customerId, clientDetails, sessionDetails)
        {
        }
    }
}
