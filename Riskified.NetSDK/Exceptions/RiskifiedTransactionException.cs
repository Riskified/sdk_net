using System;

namespace Riskified.SDK.Exceptions
{
    public class RiskifiedTransactionException : RiskifiedException
    {
        public RiskifiedTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RiskifiedTransactionException(string message) : base(message)
        {
        }
    }
}
