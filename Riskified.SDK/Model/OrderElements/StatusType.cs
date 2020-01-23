using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum StatusType
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "failure")]
        Failure
    }
}
