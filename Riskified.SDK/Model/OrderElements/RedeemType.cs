using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum RedeemType
    {
        [EnumMember(Value = "promo code")]
        PromoCode,
        [EnumMember(Value = "loyalty points")]
        LoyaltyPoints,
        [EnumMember(Value = "gift card")]
        GiftCard,
        [EnumMember(Value = "other")]
        Other
    }
}
