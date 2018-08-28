using System;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class Logout : AbstractAccountAction
    {
        public Logout(string customerId, ClientDetails clientDetails, SessionDetails sessionDetails) :
        base(customerId, clientDetails, sessionDetails)
        {
        }
    }
}
