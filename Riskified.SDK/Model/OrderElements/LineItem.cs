using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;
using System;

namespace Riskified.SDK.Model.OrderElements
{
    public class LineItem : IJsonSerializable
    {
        /// <summary>
        /// Creates a new LineItem
        /// </summary>
        /// <param name="title">A title describing the product </param>
        /// <param name="price">The product price in the currency matching the one used in the whole order and set in the "Currency" field</param>
        /// <param name="quantityPurchased">Quantity purchased of the item</param>
        /// <param name="productId">The Product ID number (optional)</param>
        /// <param name="sku">The stock keeping unit of the product (optional)</param>
        public LineItem(string title,
            double price,
            int quantityPurchased,
            //optional
            string productId = null,
            string sku = null,
            string condition = null,
            bool? requiresShipping = null,
            Seller seller = null,
            DeliveredToType? deliveredTo = null,
            DateTime? delivered_at = null,
            ProductType? productType = null,
            string brand = null,
            string category = null,
            string subCategory = null,
            Policy policy = null,
            RegistryType? registryType = null,
            Recipient recipient= null)
        {

            Title = title;
            Price = price;
            QuantityPurchased = quantityPurchased;

            // optional
            ProductId = productId;
            Sku = sku;
            Condition = condition;
            RequiresShipping = requiresShipping;
            Seller = seller;
            DeliveredTo = deliveredTo;
            ProductType = productType;
            Category = category;
            SubCategory = subCategory;
            DeliveredAt = delivered_at;
            Brand = brand;
            Policy = policy;
            RegistryType = registryType;
            Recipient = recipient;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public virtual void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateValuedString(Title, "Title");
            InputValidators.ValidateZeroOrPositiveValue(Price.Value, "Price");
            InputValidators.ValidatePositiveValue(QuantityPurchased.Value, "Quantity Purchased");

            // optional fields validations
            if (ProductId != null)
            {
                InputValidators.ValidateValuedString(ProductId, "Product Id");
            }

            if (Seller != null)
            {
                Seller.Validate(validationType);
            }
        }

        /// <summary>
        /// A title describing the product 
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// The product price in the currency matching the one used in the whole order and set in the "Currency" field
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public double? Price { get; set; }

        /// <summary>
        /// The Product ID number
        /// </summary>
        [JsonProperty(PropertyName = "product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// Quantity of the item that was purchased
        /// </summary>
        [JsonProperty(PropertyName = "quantity")]
        public int? QuantityPurchased { get; set; }

        /// <summary>
        /// States whether or not the fulfillment requires shipping. This field is important for merchants dealing with digital goods.
        /// </summary>
        [JsonProperty(PropertyName = "requires_shipping")]
        public bool? RequiresShipping { get; set; }

        /// <summary>
        /// The sku of the product
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        /// <summary>
        /// The sku of the product
        /// </summary>
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        /// <summary>
        /// Details about the seller of the item, relevant for marketplace orders.
        /// </summary>
        [JsonProperty(PropertyName = "seller")]
        public Seller Seller { get; set; }

        /// <summary>
        /// The event sub category name.
        /// </summary>
        [JsonProperty(PropertyName = "sub_category")]
        public string SubCategory { get; set; }

        /// <summary>
        /// The event category name.
        /// </summary>
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }


        [JsonProperty(PropertyName = "product_type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// The brand name of the product.
        /// </summary>
        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

        /// <summary>
        /// The devlivered_to will tell us where is the customer would like to get the product
        /// </summary>
        [JsonProperty(PropertyName = "delivered_to")]
        [JsonConverter(typeof (StringEnumConverter))]
        public DeliveredToType? DeliveredTo { get; set; }

        /// <summary>
        /// The delivery date of the goods (e.g. gift card).
        /// </summary>
        [JsonProperty(PropertyName = "delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [JsonProperty(PropertyName = "policy")]
        public Policy Policy { get; set; }


        [JsonProperty(PropertyName = "registry_type")]
        [JsonConverter(typeof (StringEnumConverter))]
        public RegistryType? RegistryType { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public Recipient Recipient { get; set; }
    }
}
