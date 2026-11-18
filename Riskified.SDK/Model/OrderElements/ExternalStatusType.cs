using System;
using System.Runtime.Serialization;

namespace Riskified.SDK.Model
{
    public enum ExternalStatusType
    {
        [EnumMember(Value = "approved")]
        Approved,
        [EnumMember(Value = "checkout")]
        [Obsolete("Checkout is removed in 6.0.0")]
        Checkout,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "declined")]
        Declined,
        [EnumMember(Value = "declined_fraud")]
        DeclinedFraud,
        [EnumMember(Value = "declined_business")]
        DeclinedBusiness,
        [EnumMember(Value = "chargeback_fraud")]
        ChargebackFraud,
        [EnumMember(Value = "chargeback_not_fraud")]
        ChargebackNotFraud,
        [EnumMember(Value = "suspected_fraud")]
        SuspectedFraud,
        [EnumMember(Value = "checkout_denied")]
        CheckoutDenied,
        [EnumMember(Value = "chargeback")]
        Chargeback
    }
}
