using Riskified.SDK.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Riskified.SDK.Model.Internal
{
    public class AuthenticationType : IJsonSerializable


    {
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "auth_type")]
        public string AuthType { get; set; }

        [JsonProperty(PropertyName = "exemption_method")]
        public string ExemptionMethod { get; set; }
    }


}