using System;

namespace Riskified.SDK.Exceptions
{
    public class RiskifiedException  : Exception
    {
        public RiskifiedException(string message) : base(message)
        {
        }

        protected RiskifiedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
