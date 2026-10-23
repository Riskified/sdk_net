using System.Runtime.Serialization;
namespace Riskified.SDK.Model.OrderElements
{
    public enum CryptoWalletType
    {
        [EnumMember(Value = "primary")]
        Primary,
        [EnumMember(Value = "secondary")]
        Secondary,
        [EnumMember(Value = "custodial")]
        Custodial,
        [EnumMember(Value = "non_custodial")]
        NonCustodial,
        [EnumMember(Value = "hardware")]
        Hardware,
        [EnumMember(Value = "exchange")]
        Exchange
    }
}
