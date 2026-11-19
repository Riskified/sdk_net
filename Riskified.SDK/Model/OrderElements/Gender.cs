using System.Runtime.Serialization;
namespace Riskified.SDK.Model.OrderElements
{
    public enum Gender
    {
        [EnumMember(Value = "female")]
        Female,
        [EnumMember(Value = "male")]
        Male
    }
}
