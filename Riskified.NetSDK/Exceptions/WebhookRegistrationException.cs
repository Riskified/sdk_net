using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.NetSDK.Exceptions
{
    public class WebhookRegistrationException : RiskifiedException
    {
        public WebhookRegistrationException(string message) : base(message)
        {
        }

        public WebhookRegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
