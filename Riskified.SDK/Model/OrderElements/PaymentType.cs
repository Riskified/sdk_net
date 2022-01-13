using System;
using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{

    public enum PaymentType
    {
        [EnumMember(Value = "paypal")]
        Paypal,
        [EnumMember(Value = "credit_card")]
        CreditCard
    }
}

