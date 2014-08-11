using System;

namespace Riskified.SDK.Exceptions
{
    public class RiskifiedException  : Exception
    {
        public RiskifiedException(string message) : base(message)
        {
        }

        public RiskifiedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
