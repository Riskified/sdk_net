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
    public class TravelTicketLineItem : LineItem
    {
        public TravelTicketLineItem(
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
            // travel ticket specific
            string legId = null,
            string departurePortCode = null,
            string departureCity = null,
            string departureCountryCode = null,
            string arrivalPortCode = null,
            string arrivalCity = null,
            string arrivalCountryCode = null,
            DateTime? departureDate = null,
            DateTime? arrivalDate = null,
            string carrierName = null,
            string carrierCode = null,
            int? routeIndex = null,
            int? legIndex = null,
            string ticketClass = null,
            TransportMethodType? transportMethod = null
            ) : base(
                title: title, price: price, quantityPurchased: quantityPurchased, productId: productId, sku: sku,
                condition: condition,
                requiresShipping: requiresShipping, seller: seller, deliveredTo: deliveredTo, delivered_at: deliveredAt,
                category: category, subCategory: subCategory, brand: brand,
                productType: OrderElements.ProductType.TravelTicket, policy: policy)
        {
            LegId = legId;
            DeparturePortCode = departurePortCode;
            DepartureCity = departureCity;
            DepartureCountryCode = departureCountryCode;
            ArrivalPortCode = arrivalPortCode;
            ArrivalCity = arrivalCity;
            ArrivalCountryCode = arrivalCountryCode;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            CarrierName = carrierName;
            CarrierCode = carrierCode;
            RouteIndex = routeIndex;
            LegIndex = legIndex;
            TicketClass = ticketClass;
            TransportMethod = transportMethod;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        /// <summary>
        /// (for travel industry) The current leg id.
        /// For flight tickets, flight number (e.g. '101').
        /// For bus tickets, bus number (e.g: 'A7')
        /// </summary>
        [JsonProperty(PropertyName = "leg_id")]
        public string LegId { get; set; }

        /// <summary>
        /// (for travel industry) Departure port code for the current leg. For flights: the 3 letter IATA airport code.
        /// </summary>
        [JsonProperty(PropertyName = "departure_port_code")]
        public string DeparturePortCode { get; set; }

        /// <summary>
        /// (for travel industry) The name of the city of departure for the current leg.
        /// </summary>
        [JsonProperty(PropertyName = "departure_city")]
        public string DepartureCity { get; set; }

        /// <summary>
        /// (for travel industry) The 2 letter country code (ISO 3166-1 alpha-2) for the departure country of the current leg.
        /// </summary>
        [JsonProperty(PropertyName = "departure_country_code")]
        public string DepartureCountryCode { get; set; }

        /// <summary>
        /// (for travel industry) Arrival port code for the current leg. For flights: the 3 letter IATA airport code.
        /// </summary>
        [JsonProperty(PropertyName = "arrival_port_code")]
        public string ArrivalPortCode { get; set; }

        /// <summary>
        /// (for travel industry) The name of the city of arrival for the current leg.
        /// </summary>
        [JsonProperty(PropertyName = "arrival_city")]
        public string ArrivalCity { get; set; }

        /// <summary>
        /// (for travel industry) The 2 letter country code (ISO 3166-1 alpha-2) for the arrival country of the current leg.
        /// </summary>
        [JsonProperty(PropertyName = "arrival_country_code")]
        public string ArrivalCountryCode { get; set; }

        /// <summary>
        /// (for travel industry) Date and time of departure for the current leg (ISO8601).
        /// </summary>
        [JsonProperty(PropertyName = "departure_date")]
        public DateTime? DepartureDate { get; set; }

        /// <summary>
        /// (for travel industry) Date and time of arrival for the current leg (ISO8601).
        /// </summary>
        [JsonProperty(PropertyName = "arrival_date")]
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// (for travel industry) The name of the carrier/company conducting the current leg.
        /// </summary>
        [JsonProperty(PropertyName = "carrier_name")]
        public string CarrierName { get; set; }

        /// <summary>
        /// (for travel industry) A publicly agreed code describing the carrier/company conducting the current leg.
        /// For Flights: The IATA 2 letter carrier code.
        /// </summary>
        [JsonProperty(PropertyName = "carrier_code")]
        public string CarrierCode { get; set; }

        /// <summary>
        /// (for travel industry) A running index (starts with 1), describing the order of routes by time.
        /// E.g: If an order contains 2 Routes:
        /// * New-York->London->Paris (connection in London)
        /// ** New-York->London should have route_index=1, leg_index=1
        /// ** London->Paris should have route_index=1, leg_index=2
        /// * Paris->London->New-York
        /// ** Paris->London should have route_index=2, leg_index=1
        /// ** London->New-York should have route_index=2, leg_index=2
        /// </summary>
        [JsonProperty(PropertyName = "route_index")]
        public int? RouteIndex { get; set; }

        /// <summary>
        /// (for travel industry) A running index (starts with 1), describing the order of legs in the same route.
        /// For more details, see route_index field.
        /// </summary>
        [JsonProperty(PropertyName = "leg_index")]
        public int? LegIndex { get; set; }

        /// <summary>
        /// (for travel industry) The class for this leg's ticket (e.g. business, economy, first)
        /// </summary>
        [JsonProperty(PropertyName = "ticket_class")]
        public string TicketClass { get; set; }

        /// <summary>
        /// (for travel industry) The method of transportation.
        /// </summary>
        [JsonProperty(PropertyName = "transport_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransportMethodType? TransportMethod { get; set; }

    }
}
