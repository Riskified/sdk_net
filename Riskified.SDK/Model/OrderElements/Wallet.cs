using System;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    [JsonObject("wallet")]
    public class Wallet : IJsonSerializable
    {
        public void Validate(Validations validationType = Validations.Weak)
        {
            throw new NotImplementedException();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}

