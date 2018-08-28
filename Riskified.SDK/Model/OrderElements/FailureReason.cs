using System.Runtime.Serialization;
namespace Riskified.SDK.Model.OrderElements
{
    public enum FailureReason
    {
        [EnumMember(Value = "wrong password")]
        WrongPassword,
        [EnumMember(Value = "captcha")]
        Captcha,
        [EnumMember(Value = "disabled account")]
        DisabledAccount,
        [EnumMember(Value = "nonexistent account")]
        NonexistentAccount,
        [EnumMember(Value = "other")]
        Other
    }
}
