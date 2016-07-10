using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

                        // These are for events tickets industry
                        string category = null,
                        string subCategory = null,
                        string eventName = null,
                        string eventSectionName = null,
                        DateTime? eventDate = null,
                        string eventCity = null,
                        string eventCountry = null,
                        string eventCountryCode = null,
                        float? latitude = null,
                        float? longitude = null,

                        // These are for digital goods (gift card) industry
                        string sender_name = null,
                        string display_name = null,
                        bool photo_uploaded = false,
                        string photo_url = null,
                        string greeting_photo_url = null,
                        string message = null,
                        string greeting_message = null,
                        string card_type = null,
                        string card_sub_type = null,
                        DateTime? delivered_at = null,
                        string sender_email = null,
                        Recipient recipient = null,

                        // These are for travel/transportaion industry
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
                        TransportMethodType? transportMethod = null)
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

            // Events Tickets Industry
            Category = category;
            SubCategory = subCategory;
            EventName = eventName;
            EventSectionName = eventSectionName;
            EventDate = eventDate;
            EventCountryCode = eventCountryCode;
            EventCity = eventCity;
            Latitude = latitude;
            Longitude = longitude;

            // Digital Goods (gift cards)
            SenderName = sender_name;
            DisplayName = display_name;
            PhotoUploaded = photo_uploaded;
            PhotoUrl = photo_url;
            GreetingPhotoUrl = greeting_photo_url;
            Message = message;
            GreetingMessage = greeting_message;
            CardType = card_type;
            CardSubtype = card_sub_type;
            DeliveredAt = delivered_at;
            SenderEmail = sender_email;

            // Recipient details 
            Recipient = recipient;

            // Travel industry
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

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
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

        [JsonProperty(PropertyName = "latitude")]
        public float? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public float? Longitude { get; set; }

        /// <summary>
        /// The brand name of the product.
        /// </summary>
        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

        /// <summary>
        /// The devlivered_to will tell us where is the customer would like to get the product
        /// </summary>
        [JsonProperty(PropertyName = "delivered_to")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveredToType? DeliveredTo { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sender name.
        /// </summary>
        [JsonProperty(PropertyName = "sender_name")]
        public string SenderName { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sender email.
        /// </summary>
        [JsonProperty(PropertyName = "sender_email")]
        public string SenderEmail { get; set; }

        /// <summary>
        /// The digital good's (giftcard) display name.
        /// </summary>
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Is the gift card sender added a photo.
        /// </summary>
        [JsonProperty(PropertyName = "photo_uploaded")]
        public bool PhotoUploaded { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sender photo's url.
        /// </summary>
        [JsonProperty(PropertyName = "photo_url")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// The digital good's (giftcard) greeting's photo url.
        /// </summary>
        [JsonProperty(PropertyName = "greeting_photo_url")]
        public string GreetingPhotoUrl { get; set; }

        /// <summary>
        /// The digital good's (giftcard) message.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// The digital good's (giftcard) greeting message.
        /// </summary>
        [JsonProperty(PropertyName = "greeting_message")]
        public string GreetingMessage { get; set; }

        /// <summary>
        /// The digital good's (giftcard) type.
        /// </summary>
        [JsonProperty(PropertyName = "card_type")]
        public string CardType { get; set; }

        /// <summary>
        /// The digital good's (giftcard) sub type.
        /// </summary>
        [JsonProperty(PropertyName = "card_subtype")]
        public string CardSubtype { get; set; }

        /// <summary>
        /// The delivery date of the goods (e.g. gift card).
        /// </summary>
        [JsonProperty(PropertyName = "delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public Recipient Recipient { get; set; }

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
