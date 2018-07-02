using System;
using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum SessionSource
    {
        [EnumMember(Value = "desktop_web")]
        DesktopWeb,
        [EnumMember(Value = "mobile_web")]
        MobileWeb,
        [EnumMember(Value = "mobile_app")]
        MobileApp,
        [EnumMember(Value = "other")]
        Other
    }
}
