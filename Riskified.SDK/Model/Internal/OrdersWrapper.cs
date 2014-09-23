using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    internal class OrdersWrapper
    {
        [JsonProperty(PropertyName = "orders", Required = Required.Always)]
        public IEnumerable<AbstractOrder> Orders { get; set; }

        public OrdersWrapper(IEnumerable<AbstractOrder> orders)
        {
            Orders = orders;
        }
    }
}
