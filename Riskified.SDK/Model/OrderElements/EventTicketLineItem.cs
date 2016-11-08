using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class EventTicketLineItem : LineItem
    {
        public EventTicketLineItem(
            // inherited
            string title,
            double price,
            int quantityPurchased,
            string productId = null,
            string sku = null,
            string condition = null,
            bool? requiresShipping = null,
            string category = null,
            string subCategory = null,
            string brand = null,
            Seller seller = null,
            DeliveredToType? deliveredTo = null,
            DateTime? deliveredAt = null,
            // event ticket specific
            string eventName = null,
            string eventSectionName = null,
            DateTime? eventDate = null,
            string eventCity = null,
            string eventCountry = null,
            string eventCountryCode = null,
            float? latitude = null,
            float? longitude = null
            ) : base(
                title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku,
                condition: condition,
                requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                category: category, subCategory: subCategory, brand: brand,
                productType: OrderElements.ProductType.EventTicket)
        {
            EventName = eventName;
            EventSectionName = eventSectionName;
            EventDate = eventDate;
            EventCountryCode = eventCountryCode;
            EventCity = eventCity;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

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
        /// The country code where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "event_country_code")]
        public string EventCountryCode { get; set; }

        /// <summary>
        /// The city where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "event_city")]
        public string EventCity { get; set; }

        /// <summary>
        /// The event date.
        /// </summary>
        [JsonProperty(PropertyName = "event_date")]
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// Latitude coordiante of the event's location
        /// </summary>
        [JsonProperty(PropertyName = "latitude")]
        public float? Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate of the event's location
        /// </summary>
        [JsonProperty(PropertyName = "longitude")]
        public float? Longitude { get; set; }
    }
}
