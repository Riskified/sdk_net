using System;
using Riskified.SDK.Utils;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class Recommendation : IJsonSerializable
    {
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "recommendation")]
        public string RecommendationText { get; set; }

        [JsonProperty(PropertyName = "recommended")]
        public bool Recommended { get; set; }
    }
}
