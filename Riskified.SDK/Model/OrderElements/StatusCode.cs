using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum StatusCode
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "error")]
        Error,
        [EnumMember(Value = "failure")]
        Failure
    }
}
