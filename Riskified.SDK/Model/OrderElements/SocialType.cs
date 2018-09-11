using System.Runtime.Serialization;
namespace Riskified.SDK.Model.OrderElements
{
    public enum SocialType
    {
        [EnumMember(Value = "facebook")]
        Facebook,
        [EnumMember(Value = "google")]
        Google,
        [EnumMember(Value = "linkedin")]
        LinkedIn,
        [EnumMember(Value = "twitter")]
        Twitter,
        [EnumMember(Value = "yahoo")]
        Yahoo,
        [EnumMember(Value = "other")]
        Other
    }
}
