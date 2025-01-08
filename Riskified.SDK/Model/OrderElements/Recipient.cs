using System;
using System.Linq;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    [JsonObject("recipient")]
    public class Recipient : IJsonSerializable
    {
        /// <summary>
        /// Creates a new Recipient, mainly for digital goods (e.g. giftcards) orders
        /// </summary>
        /// <param name="email">The recipient email</param>
        /// <param name="notes">Additional notes regarding the customer (optional)</param>
        public Recipient(string email = null, string phone = null, SocialDetails social = null)
        {
            Email = email;
            Social = social;
            Phone = phone;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this Model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            // optional fields validations
            if (!string.IsNullOrEmpty(Email))
            {
                InputValidators.ValidateEmail(Email);
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                InputValidators.ValidatePhoneNumber(Phone);
            }
            if (Social != null)
            {
                Social.Validate(validationType);
            }
            if (Social==null && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone))
            {
                throw new Exceptions.OrderFieldBadFormatException("All recipient fields are missing - at least one should be specified");
            }
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "social")]
        public SocialDetails Social { get; set; }

        [JsonProperty(PropertyName = "account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "routing_number")]
        public string RoutingNumber { get; set; }
    }
}
