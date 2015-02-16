using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum CancelReason
    {
        [EnumMember(Value = "customer")]
        Customer,
        [EnumMember(Value = "fraud")]
        Fraud,
        [EnumMember(Value = "inventory")]
        Inventory,
        [EnumMember(Value = "other")]
        Other
    }
}
