using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class AccommodationLineItem : LineItem
    {
        public AccommodationLineItem(
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
            Recipient recipient = null,
            // accommodation specific
            string roomType = null,
            string city = null,
            string countryCode = null,
            DateTime? checkInDate = null,
            DateTime? checkOutTime = null,
            string rating = null,
            ushort? numberOfGuests = null,
            string cancellationPolicy = null,
            string accommodationType = null
            ) : base(title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku, condition: condition, 
                     requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                     category: category, subCategory: subCategory, brand: brand, productType: OrderElements.ProductType.Accommodation, policy: policy, recipient: recipient)
        {
            RoomType = roomType;
            City = city;
            CountryCode = countryCode;
            CheckInDate = checkInDate;
            CheckOutTime = checkOutTime;
            Rating = rating;
            NumberOfGuests = numberOfGuests;
            CancellationPolicy = cancellationPolicy;
            AccommodationType = accommodationType;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);

            if (CountryCode != null) InputValidators.ValidateCountryOrProvinceCode(CountryCode);
        }

        [JsonProperty(PropertyName = "room_type")]
        public string RoomType { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "check_in_date")]
        public DateTime? CheckInDate { get; set; }

        [JsonProperty(PropertyName = "check_out_date")]
        public DateTime? CheckOutTime { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "number_of_guests")]
        public ushort? NumberOfGuests { get; set; }

        [JsonProperty(PropertyName = "cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty(PropertyName = "accommodation_type")]
        public string AccommodationType { get; set; }

    }
}
