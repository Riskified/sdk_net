using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{

    public class ShippingLine
    {
        /// <summary>
        /// The shipping line (shiiping method)
        /// </summary>
        /// <param name="price">The price of this shipping method</param>
        /// <param name="title">A human readable name for the shipping method</param>
        /// <param name="code">A code to the shipping method</param>
        public ShippingLine(double price, string title, string code = null)
        {
            Code = code;
            Price = price;
            Title = title;
        }

        [JsonProperty(PropertyName = "code", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double? Price { get; set; }

        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; set; }
    }

}
