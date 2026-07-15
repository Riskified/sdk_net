using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum AuthorizationType
    {
        [EnumMember(Value = "verification")]
        Verification,
        [EnumMember(Value = "authorization")]
        Authorization
    }
}
