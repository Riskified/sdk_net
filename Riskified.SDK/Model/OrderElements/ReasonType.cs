using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum ReasonType
    {
        [EnumMember(Value = "user_requested")]
        UserRequested,
        [EnumMember(Value = "forgot_password")]
        ForgotPassword,
        [EnumMember(Value = "forced_reset")]
        ForcedReset
    }
}
