using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum PaymentType
    {
        [EnumMember(Value = "card")]
        Card,
        [EnumMember(Value = "apple_pay")]
        ApplePay,
        [EnumMember(Value = "google_pay")]
        GooglePay,
        [EnumMember(Value = "samsung_pay")]
        SamsungPay,
        [EnumMember(Value = "paypal")]
        Paypal,
        [EnumMember(Value = "shop_pay")]
        ShopPay,
        [EnumMember(Value = "amazon_pay")]
        AmazonPay,
        [EnumMember(Value = "bnpl")]
        Bnpl,
        [EnumMember(Value = "bank_transfer")]
        BankTransfer,
        [EnumMember(Value = "ach")]
        Ach,
        [EnumMember(Value = "crypto")]
        Crypto,
        [EnumMember(Value = "gift_card")]
        GiftCard,
        [EnumMember(Value = "store_credit")]
        StoreCredit,
        [EnumMember(Value = "cash_on_delivery")]
        CashOnDelivery,
        [EnumMember(Value = "invoice")]
        Invoice,
        /// <summary>
        /// Sent as reward_points. The API reference documents rewards_points; see <see cref="RewardsPoints"/>.
        /// </summary>
        [EnumMember(Value = "reward_points")]
        RewardPoints,
        [EnumMember(Value = "mobile_carrier")]
        MobileCarrier,
        [EnumMember(Value = "cashe")]
        Cashe,
        [EnumMember(Value = "check")]
        Check,
        [EnumMember(Value = "boleto")]
        Boleto,
        [EnumMember(Value = "wechat_pay")]
        WechatPay,
        [EnumMember(Value = "alipay")]
        Alipay,
        [EnumMember(Value = "other")]
        Other,
        /// <summary>
        /// Reward points, sent as rewards_points (the documented value, e.g. for <see cref="NonChbPaymentDetails"/>).
        /// </summary>
        [EnumMember(Value = "rewards_points")]
        RewardsPoints
    }
}