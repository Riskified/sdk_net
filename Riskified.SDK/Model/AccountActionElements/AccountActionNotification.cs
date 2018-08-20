using System;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class AccountActionNotification
    {
        [JsonProperty(PropertyName = "decision", Required = Required.Always)]
        public string Decision { get; set; }
    }
}
