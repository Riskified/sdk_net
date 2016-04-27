using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    public class OrdersWrapper
    {
        [JsonProperty(PropertyName = "orders")]
        public IEnumerable<AbstractOrder> Orders { get; set; }

        public OrdersWrapper(IEnumerable<AbstractOrder> orders)
        {
            Orders = orders;
        }
    }
}
