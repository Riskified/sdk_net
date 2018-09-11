using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class CustomerCreate : BaseCustomerAction
    {
        public CustomerCreate(string customerId, ClientDetails clientDetails, SessionDetails sessionDetails, Customer customer) :
        base(customerId, clientDetails, sessionDetails, customer)
        {
        }
    }
}
