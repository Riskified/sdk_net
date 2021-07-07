using System;
using Riskified.SDK.Utils;
using Newtonsoft.Json;
namespace Riskified.SDK.Model.Internal
{
    public class Advice : IJsonSerializable
    {
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "in_regulatory_scope")]
        public bool RegulatoryScope { get; set; }

        [JsonProperty(PropertyName = "recommendation")]
        public string Recommendation { get; set; }
    }
}


