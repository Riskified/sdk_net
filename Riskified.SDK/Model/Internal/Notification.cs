using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Model.PolicyElements;

//Shop URL is available as a notification parameter depending on your account's setup; please contact your Integration Engineer or Account Manager if you have questions on this.
//It is a NON-best-practice to use shop URL in the notifications programmatically as this field will not be supported long term in API notifications.

namespace Riskified.SDK.Model.Internal
{
    internal class Notification
    {


        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "oldStatus", Required = Required.Default)]
        public string OldStatus { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "custom", Required = Required.Default)]
        public Custom Custom { get; set; }

        [JsonProperty(PropertyName = "category", Required = Required.Default)]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "action", Required = Required.Default)]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "decision_code", Required = Required.Default)]
        public string DecisionCode { get; set; }
      
        [JsonProperty(PropertyName = "score", Required = Required.Default)]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "authentication_type", Required = Required.Default)]
        public AuthenticationType AuthenticationType { get; set; }

        [JsonProperty(PropertyName = "policy_protect", Required = Required.Default)]
        public PolicyProtect PolicyProtect  { get; set; }


    }
}
