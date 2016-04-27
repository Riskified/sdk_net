using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.Internal
{
    public class OrderCheckoutWrapper<TOrderCheckout>
    {
        [JsonProperty(PropertyName = "checkout", Required = Required.Always)]
        public TOrderCheckout Order { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public string[] Warnings { get; set; }

        public OrderCheckoutWrapper(TOrderCheckout order)
        {
            Order = order;
        }
    }
}
