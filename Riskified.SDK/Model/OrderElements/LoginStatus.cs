using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Riskified.SDK.Model.OrderElements
{
    public class LoginStatus
    {
        public LoginStatus(LoginStatusType loginStatusType)
        {
            LoginStatusType = loginStatusType;
        }

        [JsonProperty(PropertyName = "login_status_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LoginStatusType LoginStatusType { get; set; }

        [JsonProperty(PropertyName = "failure_reason")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FailureReason? FailureReason { get; set; }
    }
}
