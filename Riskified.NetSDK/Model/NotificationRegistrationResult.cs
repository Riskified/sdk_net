using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    internal class NotificationRegistrationResult
    {
        [JsonProperty(PropertyName = "action_succeeded",Required = Required.Always,NullValueHandling = NullValueHandling.Ignore)]
        public bool IsActionSucceeded { get; set; }

        [JsonProperty(PropertyName = "message", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

    }
}
