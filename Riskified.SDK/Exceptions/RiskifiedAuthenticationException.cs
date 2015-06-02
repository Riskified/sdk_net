using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Exceptions
{
    public class RiskifiedAuthenticationException : RiskifiedException
    {
        public RiskifiedAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RiskifiedAuthenticationException(string message)
            : base(message)
        {
        }
    }
}
