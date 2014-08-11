using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
