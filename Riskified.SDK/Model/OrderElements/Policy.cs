using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class Policy : IJsonSerializable
    {
        public Policy(bool? evaluate)
        {
            Evaluate = evaluate;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
            return;
        }

        [JsonProperty(PropertyName = "evaluate")]
        public bool? Evaluate { get; set; }
    }
}
