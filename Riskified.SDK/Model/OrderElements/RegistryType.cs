using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum RegistryType
    {
        [EnumMember(Value = "wedding")]
        Wedding,
        [EnumMember(Value = "baby")]
        Baby,
        [EnumMember(Value = "other")]
        Other
    }
}
