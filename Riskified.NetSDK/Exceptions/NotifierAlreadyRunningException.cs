using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Exceptions
{
    public class NotifierAlreadyRunningException : RiskifiedException
    {
        public NotifierAlreadyRunningException(string message) : base(message)
        {
        }

        public NotifierAlreadyRunningException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
