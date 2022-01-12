using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class RideTicketLineItem : LineItem
    {
        public RideTicketLineItem(
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
            // ride item specific
            DateTime? pickupDate = null,
            float? pickupLatitude = null,
            float? pickupLongitude = null,
            BasicAddress pickupAddress = null,
            DateTime? dropoffDate = null,
            float? dropoffLatitude = null,
            float? dropoffLongitude = null,
            BasicAddress dropoffAddress = null,
            string transportMethod = null,
            string priceBy = null,
            string vehicleClass = null,
            string carrierName = null,
            string driverId = null,
            string tariff = null,
            string noteToDriver = null,
            string meetNGreet = null,
            string cancellationPolicy = null,
            float? authorizedPayments = null,
            int? routeIndex = null,
            int? legIndex = null
            ) : base(
                 title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku,
                condition: condition,
                requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                category: category, subCategory: subCategory, brand: brand,
                productType: OrderElements.ProductType.RideTicket, policy: policy)
        {
            PickupDate = pickupDate;
            PickupLatitude = pickupLatitude;
            PickupLongitude = pickupLongitude;
            PickupAddress = pickupAddress;
            DropoffDate = dropoffDate;
            DropoffLatitude = dropoffLatitude;
            DropoffLongitude = dropoffLongitude;
            DropoffAddress = dropoffAddress;
            TransportMethod = transportMethod;
            PriceBy = priceBy;
            VehicleClass = vehicleClass;
            CarrierName = carrierName;
            DriverId = driverId;
            Tariff = tariff;
            NoteToDriver = noteToDriver;
            MeetNGreet = meetNGreet;
            CancellationPolicy = cancellationPolicy;
            AuthorizedPayments = authorizedPayments;
            RouteIndex = routeIndex;
            LegIndex = legIndex;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        [JsonProperty(PropertyName = "pickup_date")]
        public DateTime? PickupDate { get; set; }

        [JsonProperty(PropertyName = "pickup_latitude")]
        public float? PickupLatitude { get; set; }

        [JsonProperty(PropertyName = "pickup_longitude")]
        public float? PickupLongitude { get; set; }

        [JsonProperty(PropertyName = "pickup_address")]
        public BasicAddress PickupAddress { get; set; }

        [JsonProperty(PropertyName = "dropoff_date")]
        public DateTime? DropoffDate { get; set; }

        [JsonProperty(PropertyName = "dropoff_latitiude")]
        public float? DropoffLatitude { get; set; }

        [JsonProperty(PropertyName = "dropoff_longitude")]
        public float? DropoffLongitude { get; set; } 

        [JsonProperty(PropertyName = "dropoff_address")]
        public BasicAddress DropoffAddress { get; set; } 

        [JsonProperty(PropertyName = "transport_method")]
        public string TransportMethod { get; set; }

        [JsonProperty(PropertyName = "price_by")]
        public string PriceBy { get; set; }

        [JsonProperty(PropertyName = "vehicle_class")]
        public string VehicleClass { get; set; }

        [JsonProperty(PropertyName = "carrier_name")]
        public string CarrierName { get; set; }

        [JsonProperty(PropertyName = "driver_id")]
        public string DriverId { get; set; }

        [JsonProperty(PropertyName = "tariff")]
        public string Tariff { get; set; }

        [JsonProperty(PropertyName = "note_to_driver")]
        public string NoteToDriver { get; set; }

        [JsonProperty(PropertyName = "meet_n_greet")]
        public string MeetNGreet { get; set; }

        [JsonProperty(PropertyName = "cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty(PropertyName = "authorized_payments")]
        public float? AuthorizedPayments { get; set; }

        [JsonProperty(PropertyName = "route_index")]
        public int? RouteIndex { get; set; }

        [JsonProperty(PropertyName = "leg_index")]
        public int? LegIndex { get; set; } 

    }
}