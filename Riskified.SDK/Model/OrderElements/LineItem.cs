using Newtonsoft.Json;
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
        public LineItem(string title, double price, int quantityPurchased, int? productId = null, string sku = null, string condition = null, bool? requiresShipping = null, Seller seller = null,
                    string eventSubCategoryName = null, string eventName = null, string eventSectionName = null, DateTime? eventDate = null)
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
            EventSubCategoryName = eventSubCategoryName;
            EventName = eventName;
            EventSectionName = eventSectionName;
            EventDate = eventDate;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateValuedString(Title, "Title");
            InputValidators.ValidateZeroOrPositiveValue(Price.Value, "Price");
            InputValidators.ValidatePositiveValue(QuantityPurchased.Value, "Quantity Purchased");

            // optional fields validations
            if(ProductId.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(ProductId.Value, "Product Id");
            }

            if(Seller != null)
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
        public int? ProductId { get; set; }

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
        [JsonProperty(PropertyName = "event_sub_category_name")]
        public string EventSubCategoryName { get; set; }


        /// <summary>
        /// The event category name.
        /// </summary>
        [JsonProperty(PropertyName = "event_category_name")]
        public string EventCategoryName { get; set; }

        /// <summary>
        /// The event name.
        /// </summary>
        [JsonProperty(PropertyName = "event_name")]
        public string EventName { get; set; }

        /// <summary>
        /// The event section name.
        /// </summary>
        [JsonProperty(PropertyName = "event_section_name")]
        public string EventSectionName { get; set; }

        /// <summary>
        /// The country where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "event_country")]
        public string EventCountry { get; set; }

        /// <summary>
        /// The city where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "event_city")]
        public string EventCity { get; set; }

        /// <summary>
        /// The geographic coordinates (Decimal degrees) where the event is taking place. (United States capitol for example is 38.8897,-77.0089).
        /// </summary>
        [JsonProperty(PropertyName = "event_location")]
        public string EventLocation { get; set; }

        /// <summary>
        /// The event date.
        /// </summary>
        [JsonProperty(PropertyName = "event_date")]
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// The brand name of the product.
        /// </summary>
        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

    }

}
