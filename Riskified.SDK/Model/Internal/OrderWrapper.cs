using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    public class OrderWrapper<TOrder>
    {
        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public TOrder Order { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public string[] Warnings { get; set; }

        public OrderWrapper(TOrder order)
        {
            Order = order;
        }
    }
}
