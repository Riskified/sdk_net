using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Riskified.SDK.Model
{
    public enum TransportMethodType
    {
        [EnumMember(Value = "plane")]
        Plane,
        [EnumMember(Value = "ship")]
        Ship,
        [EnumMember(Value = "bus")]
        Bus,
        [EnumMember(Value = "train")]
        Train
    }
}
