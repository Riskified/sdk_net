using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public enum ProductType
    {
        [EnumMember(Value = "digital")]
        Digital,
        [EnumMember(Value = "downloadable")]
        Downloadable,
        [EnumMember(Value = "physical")]
        Physical,
        [EnumMember(Value = "composite")]
        Composite,
        [EnumMember(Value = "event")]
        EventTicket,
        [EnumMember(Value = "travel")]
        TravelTicket,
        [EnumMember(Value = "accommodation")]
        Accommodation,
        [EnumMember(Value = "ride")]
        RideTicket,
        [EnumMember(Value = "remittance")]
        Remittance
    }
}
