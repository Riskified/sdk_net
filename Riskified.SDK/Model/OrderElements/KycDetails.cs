using System;
using System.Runtime.Serialization;
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

        // The docs spell this key update_at; updated_at is still sent too. Not sent with the
        // compatibility switch on (5.7.x didn't send it).
        [JsonProperty(PropertyName = "update_at")]
        private DateTimeOffset? UpdateAtWire
        {
            get { return _legacyWireFormat ? null : UpdatedAt; }
            set
            {
                if (value != null)
                {
                    UpdatedAt = value;
                }
            }
        }

        private bool _legacyWireFormat;

        [OnSerializing]
        private void OnSerializingKycDetails(StreamingContext context)
        {
            _legacyWireFormat = WireOptions.IsLegacy(context);
        }

        [JsonProperty(PropertyName = "kyc_verified")]
        public bool KycVerified { get; set; }

        [JsonProperty(PropertyName = "kyc_type")]
        public string KycType { get; set; }
    }
}
