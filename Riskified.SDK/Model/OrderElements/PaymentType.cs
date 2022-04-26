using System;
using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    [Obsolete("payment_type not in use anymore", true)]
    public enum PaymentType
    {
        [EnumMember(Value = "paypal")]
        Paypal,
        [EnumMember(Value = "credit_card")]
        CreditCard
    }
}

