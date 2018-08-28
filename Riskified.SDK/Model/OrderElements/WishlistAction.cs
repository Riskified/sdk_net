using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum WishlistAction
    {
        [EnumMember(Value = "add")]
        Add,
        [EnumMember(Value = "remove")]
        Remove
    }
}
