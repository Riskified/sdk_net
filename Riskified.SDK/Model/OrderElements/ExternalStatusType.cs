using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    public enum ExternalStatusType
    {
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "checkout")]
        Checkout,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "declined")]
        Declined,
        [EnumMember(Value = "declined_fraud")]
        DeclinedFraud,
        [EnumMember(Value = "chargeback_fraud")]
        ChargebackFraud,
        [EnumMember(Value = "chargeback_not_fraud")]
        ChargebackNotFraud
    }
}
