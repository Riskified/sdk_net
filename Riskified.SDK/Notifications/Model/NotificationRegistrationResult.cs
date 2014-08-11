using Newtonsoft.Json;

namespace Riskified.SDK.Notifications.Model
{
    public class NotificationRegistrationResult
    {
        [JsonProperty(PropertyName = "registration_result", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public NotificationResultMessage SuccessfulResult { get; set; }

        [JsonProperty(PropertyName = "error", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public NotificationResultMessage FailedResult { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get { return SuccessfulResult != null; } }

    }

    public class NotificationResultMessage
    {
        [JsonProperty(PropertyName = "message", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
