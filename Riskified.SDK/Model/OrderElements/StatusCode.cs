using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum StatusCode
    {
        [Description("success")]
        Success,
        [Description("cancelled")]
        Cancelled,
        [Description("error")]
        Error,
        [Description("failure")]
        Failure
    }
}
