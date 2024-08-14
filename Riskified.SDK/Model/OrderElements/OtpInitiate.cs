using System;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class OtpInitiate : IJsonSerializable
    {
        public OtpInitiate(string id, string challengeAccessToken, string localizationLanguage, string contactDetails, ChannelMethod channelMethod)
        {
            Id = id;
            ChallengeAccesstoken = challengeAccessToken;
            LocalizationLanguage = localizationLanguage;
            ContactDetails = contactDetails;
            ChannelMethod = channelMethod;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "challenge_access_token")]
        public string ChallengeAccesstoken { get; set; }

        [JsonProperty(PropertyName = "localization_language")]
        public string LocalizationLanguage { get; set; }

        [JsonProperty(PropertyName = "contact_details")]
        public string ContactDetails { get; set; }

        [JsonProperty(PropertyName = "channel_method")]
        public ChannelMethod ChannelMethod { get; set; }

        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateValuedString(Id, "Merchant Order ID");
        }
    }
}
