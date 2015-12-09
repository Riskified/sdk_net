using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;

namespace Riskified.SDK.Model.OrderElements
{
    public class ClientDetails : IJsonSerializable
    {
        /// <summary>
        /// Technical information regarding the customer's browsing session 
        /// </summary>
        /// <param name="accept_language">List of two-letter language codes sent from the client</param>
        /// <param name="user_agent">The full User-Agent sent from the client</param>
        public ClientDetails(string accept_language = null, 
                             string user_agent = null)
        {
            this.AcceptLanguage = accept_language;
            this.UserAgent = user_agent;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
            return;
        }

        [JsonProperty(PropertyName = "accept_language")]
        public string AcceptLanguage { get; set; }

        [JsonProperty(PropertyName = "user_agent")]
        public string UserAgent { get; set; }
    }
}
