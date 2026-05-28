using System.Collections.Generic;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// Represents identity information associated with a payment account.
    /// All fields are optional.
    /// </summary>
    public class AccountIdentity : IJsonSerializable
    {
        public AccountIdentity() { }

        /// <summary>
        /// Validates the object's fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            if (Addresses != null)
            {
                foreach (var address in Addresses)
                {
                    address.Validate(validationType);
                }
            }
        }

        [JsonProperty(PropertyName = "names")]
        public List<string> Names { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public List<BasicAddress> Addresses { get; set; }

        [JsonProperty(PropertyName = "phone_numbers")]
        public List<string> PhoneNumbers { get; set; }

        [JsonProperty(PropertyName = "emails")]
        public List<string> Emails { get; set; }
    }
}
