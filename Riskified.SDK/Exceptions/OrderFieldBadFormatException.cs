using System;

namespace Riskified.SDK.Exceptions
{
    public class OrderFieldBadFormatException : RiskifiedException
    {
        public OrderFieldBadFormatException(string message) : base(message)
        {
        }

        public OrderFieldBadFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
