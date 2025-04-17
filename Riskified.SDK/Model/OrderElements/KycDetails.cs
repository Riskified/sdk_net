using System;
using Riskified.SDK.Utils;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class KycDetails: IJsonSerializable
    {

        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "vendor_name")]
        public string VendorName { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "kyc_verified")]
        public bool KycVerified { get; set; }

        [JsonProperty(PropertyName = "kyc_type")]
        public string KycType { get; set; }
    }
}
