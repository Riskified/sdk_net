using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{

    public class ShippingLine
    {

        [JsonProperty(PropertyName = "code", Required = Required.Default)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; set; }
    }

}
