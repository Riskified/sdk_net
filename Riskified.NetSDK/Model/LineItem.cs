using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    public class LineItem
    {
        /// <summary>
        /// Creates a new LineItem
        /// </summary>
        /// <param name="title">A title describing the product </param>
        /// <param name="price">The product price in the currency matching the one used in the whole order and set in the "Currency" field</param>
        /// <param name="quantityPurchased">Quantity purchased of the item</param>
        /// <param name="productId">The Product ID number</param>
        /// <param name="sku">The stock keeping unit of the product</param>
        public LineItem(string title, double price, int quantityPurchased,int productId=0,string sku=null)
        {
            Title = title;
            Price = price;
            QuantityPurchased = quantityPurchased;
            ProductId = productId == 0 ? (int?) null : productId;
            Sku = sku;
        }

        /// <summary>
        /// A title describing the product 
        /// </summary>
        [JsonProperty(PropertyName = "title",Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// The product price in the currency matching the one used in the whole order and set in the "Currency" field
        /// </summary>
        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double? Price { get; set; }

        /// <summary>
        /// The Product ID number
        /// </summary>
        [JsonProperty(PropertyName = "product_id", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public int? ProductId { get; set; }

        /// <summary>
        /// Quantity of the item that was purchased
        /// </summary>
        [JsonProperty(PropertyName = "quantity", Required = Required.Always)]
        public int? QuantityPurchased { get; set; }

        /// <summary>
        /// The sku of the product
        /// </summary>
        [JsonProperty(PropertyName = "sku", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Sku { get; set; }
    }

}
