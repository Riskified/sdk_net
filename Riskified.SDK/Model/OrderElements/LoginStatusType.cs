using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum LoginStatusType
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "failure")]
        Failure
    }
}
