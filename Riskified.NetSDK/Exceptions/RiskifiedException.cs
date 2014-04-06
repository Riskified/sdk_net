using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Exceptions
{
    public class RiskifiedException  : Exception
    {
        protected RiskifiedException(string message) : base(message)
        {
        }

        public RiskifiedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
