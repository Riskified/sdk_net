using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    public class LineItem
    {

        [JsonProperty(PropertyName = "title",Required = Required.Always)]
        public string ProductTitle { get; set; }

        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "product_id", Required = Required.Default)]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = "quantity", Required = Required.Always)]
        public int QuantityPurchased { get; set; }

        [JsonProperty(PropertyName = "sku", Required = Required.Default)]
        public string Sku { get; set; }
    }

}
