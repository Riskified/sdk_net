using Newtonsoft.Json;

namespace Riskified.SDK.Model.OtpElements
{
    public class OtpWidgetNotification
    {
        [JsonProperty(PropertyName = "widget_token", Required = Required.Always)]
        public string WidgetToken { get; set; }
    }
}
