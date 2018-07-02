using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum ContactMethodType
    {
        [EnumMember(Value = "email")]
        Email,
        [EnumMember(Value = "website_chat")]
        WebsiteChat,
        [EnumMember(Value = "facebook")]
        Facebook,
        [EnumMember(Value = "phone")]
        Phone,
        [EnumMember(Value = "other")]
        Other
    }
}
