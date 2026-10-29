using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// The affiliated partner of a line item, when each line item can have a different one.
    /// </summary>
    public class Vendor : IJsonSerializable
    {
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "vendor_id")]
        public string VendorId { get; set; }

        [JsonProperty(PropertyName = "vendor_name")]
        public string VendorName { get; set; }
    }
}
