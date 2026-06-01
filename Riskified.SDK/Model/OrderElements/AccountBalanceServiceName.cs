using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum AccountBalanceServiceName
    {
        [EnumMember(Value = "plaid")]
        Plaid,
        [EnumMember(Value = "mx")]
        Mx,
        [EnumMember(Value = "stripe")]
        Stripe,
        [EnumMember(Value = "truelayer")]
        Truelayer,
        [EnumMember(Value = "klarna")]
        Klarna,
        [EnumMember(Value = "visa")]
        Visa,
        [EnumMember(Value = "mastercard")]
        Mastercard,
        [EnumMember(Value = "yodlee")]
        Yodlee
    }
}
