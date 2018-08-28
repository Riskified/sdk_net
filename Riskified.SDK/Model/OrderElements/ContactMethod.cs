using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Riskified.SDK.Model.OrderElements
{
    public class ContactMethod
    {
        public ContactMethod(ContactMethodType contactMethodType)
        {
            ContactMethodType = contactMethodType;
        }

        [JsonProperty(PropertyName = "contact_method_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactMethodType ContactMethodType { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "facebook_account_url")]
        public string FacebookAccountUrl { get; set; }

        [JsonProperty(PropertyName = "number_of_messages")]
        public string NumberOfMessages { get; set; }

        [JsonProperty(PropertyName = "chat_subject")]
        public string ChatSubject { get; set; }
    }
}
