using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Exceptions
{
    public class RiskifiedGatewayException : Exception
    {
        public RiskifiedGatewayException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
