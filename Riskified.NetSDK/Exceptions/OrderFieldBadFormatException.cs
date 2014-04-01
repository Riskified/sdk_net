using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Exceptions
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
