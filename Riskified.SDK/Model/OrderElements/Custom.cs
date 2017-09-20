using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;

namespace Riskified.SDK.Model.OrderElements
{
    public class Custom : IJsonSerializable
    {
        /// <summary>
        /// Custom information attached to the order 
        /// </summary>
        /// <param name="app_dom_id">Originating System</param>
        public Custom(string app_dom_id = null)
        {
            this.AppDomId = app_dom_id;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
            return;
        }

        [JsonProperty(PropertyName = "app_dom_id")]
        public string AppDomId { get; set; }
    }
}