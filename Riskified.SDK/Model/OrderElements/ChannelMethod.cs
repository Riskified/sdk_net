using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class ChannelMethod
    {
        public ChannelMethod(ChannelType channelType, string senderName)
        {
            ChannelType = channelType;
            SenderName = senderName;
        }

        [JsonProperty(PropertyName = "channel_type")]
        public ChannelType ChannelType { get; set; }

        [JsonProperty(PropertyName = "sender_name")]
        public string SenderName { get; set; }
    }
}