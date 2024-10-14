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
            Policy policy = null, 
            // event ticket specific
            Recipient recipient = null,
            string section = null,
            DateTime? eventDate = null,
            string city = null,
            string countryCode = null,
            float? latitude = null,
            float? longitude = null,
            string message = null
            ) : base(
                title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku,
                condition: condition,
                requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                category: category, subCategory: subCategory, brand: brand,
                productType: OrderElements.ProductType.EventTicket, policy: policy, recipient: recipient, message: message)
        {
            Section = section;
            EventDate = eventDate;
            CountryCode = countryCode;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }


        /// <summary>
        /// The event section name.
        /// </summary>
        [JsonProperty(PropertyName = "section")]
        public string Section { get; set; }

        /// <summary>
        /// The country code where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// The city where the event is taking place.
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

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
