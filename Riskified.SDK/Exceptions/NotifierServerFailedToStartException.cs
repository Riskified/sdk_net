using System;

namespace Riskified.SDK.Exceptions
{
    public class NotifierServerFailedToStartException :RiskifiedException
    {
        public NotifierServerFailedToStartException(string message)
            : base(message)
        {
        }

        public NotifierServerFailedToStartException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
