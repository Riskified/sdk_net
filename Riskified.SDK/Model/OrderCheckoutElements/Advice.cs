using System;
using Riskified.SDK.Utils;
using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.Internal
{
    public class Advice : IJsonSerializable
    {
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "in_regulatory_scope")]
        public bool RegulatoryScope { get; set; }

        [JsonProperty(PropertyName = "recommendations")]
        public Recommendation[] Recommendations { get; set; }

        [JsonProperty(PropertyName = "safe_order")]
        public bool SafeOrder { get; set; }

    }
}
