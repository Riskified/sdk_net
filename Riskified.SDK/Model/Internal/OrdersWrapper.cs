using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    internal class OrdersWrapper<TOrder>
    {
        [JsonProperty(PropertyName = "orders")]
        public IEnumerable<TOrder> Orders { get; set; }

        public OrdersWrapper(IEnumerable<TOrder> orders)
        {
            Orders = orders;
        }
    }
}
