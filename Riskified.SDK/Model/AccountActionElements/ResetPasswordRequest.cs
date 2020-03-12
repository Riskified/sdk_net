using Riskified.SDK.Model.OrderElements;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class ResetPasswordRequest : AbstractAccountAction
    {
        public ResetPasswordRequest(string customerId, StatusType status, ReasonType reason, string email, ClientDetails clientDetails, SessionDetails sessionDetails) :
        base(customerId, clientDetails, sessionDetails)
        {
            Status = status;
            Reason = reason;
            Email = email; 
        }

        [JsonProperty(PropertyName = "status")]
        public StatusType Status { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public ReasonType Reason { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
