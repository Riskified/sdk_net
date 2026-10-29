using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
            SingleOrArray.SetFirst(_pickupAddresses, pickupAddress);
            DropoffDate = dropoffDate;
            DropoffLatitude = dropoffLatitude;
            DropoffLongitude = dropoffLongitude;
            SingleOrArray.SetFirst(_dropoffAddresses, dropoffAddress);
            TransportMethod = transportMethod;
            PriceBy = priceBy;
            VehicleClass = vehicleClass;
            CarrierName = carrierName;
            DriverId = driverId;
            Tariff = tariff;
            NoteToDriver = noteToDriver;
            MeetNGreet = meetNGreet;
            CancellationPolicy = cancellationPolicy;
            AuthorizedPayment = authorizedPayments;
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

        // pickup_address / dropoff_address are documented as arrays. The single and array properties share
        // one list each, and the private *Wire properties send it (see OrderBase.BillingAddress).
        private readonly List<BasicAddress> _pickupAddresses = new List<BasicAddress>();
        private readonly List<BasicAddress> _dropoffAddresses = new List<BasicAddress>();
        private bool _legacyWireFormat;

        /// <summary>
        /// The pickup address: the first of <see cref="PickupAddresses"/>. Setting it replaces the first
        /// pickup address; null removes it.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use PickupAddresses. PickupAddress is the first pickup address; it's removed in 6.0.0.")]
        public BasicAddress PickupAddress
        {
            get { return SingleOrArray.First(_pickupAddresses); }
            set { SingleOrArray.SetFirst(_pickupAddresses, value); }
        }

        /// <summary>
        /// The pickup addresses. The getter returns a copy; assign a new array to change them.
        /// </summary>
        [JsonIgnore]
        public BasicAddress[] PickupAddresses
        {
            get { return SingleOrArray.ToArray(_pickupAddresses); }
            set { SingleOrArray.ReplaceAll(_pickupAddresses, value); }
        }

        // The compatibility switch sends the 5.7.x single object.
        [JsonProperty(PropertyName = "pickup_address")]
        private object PickupAddressWire
        {
            get { return SingleOrArray.ToWire(_pickupAddresses, _legacyWireFormat); }
            set { SingleOrArray.FromWire(_pickupAddresses, value); }
        }

        [JsonProperty(PropertyName = "dropoff_date")]
        public DateTime? DropoffDate { get; set; }

        [JsonProperty(PropertyName = "dropoff_latitiude")]
        public float? DropoffLatitude { get; set; }

        // The documented spelling; the misspelled key above is still sent too. Not sent with the
        // compatibility switch on (5.7.x didn't send it).
        [JsonProperty(PropertyName = "dropoff_latitude")]
        private float? DropoffLatitudeWire
        {
            get { return _legacyWireFormat ? null : DropoffLatitude; }
            set
            {
                if (value != null)
                {
                    DropoffLatitude = value;
                }
            }
        }

        [JsonProperty(PropertyName = "dropoff_longitude")]
        public float? DropoffLongitude { get; set; } 

        /// <summary>
        /// The drop-off address: the first of <see cref="DropoffAddresses"/>. Setting it replaces the first
        /// drop-off address; null removes it.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use DropoffAddresses. DropoffAddress is the first drop-off address; it's removed in 6.0.0.")]
        public BasicAddress DropoffAddress
        {
            get { return SingleOrArray.First(_dropoffAddresses); }
            set { SingleOrArray.SetFirst(_dropoffAddresses, value); }
        }

        /// <summary>
        /// The drop-off addresses. The getter returns a copy; assign a new array to change them.
        /// </summary>
        [JsonIgnore]
        public BasicAddress[] DropoffAddresses
        {
            get { return SingleOrArray.ToArray(_dropoffAddresses); }
            set { SingleOrArray.ReplaceAll(_dropoffAddresses, value); }
        }

        [JsonProperty(PropertyName = "dropoff_address")]
        private object DropoffAddressWire
        {
            get { return SingleOrArray.ToWire(_dropoffAddresses, _legacyWireFormat); }
            set { SingleOrArray.FromWire(_dropoffAddresses, value); }
        }

        [OnSerializing]
        private void OnSerializingRideTicketLineItem(StreamingContext context)
        {
            _legacyWireFormat = WireOptions.IsLegacy(context);
        }

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
        [Obsolete("Use AuthorizedPayment, the documented name (same value); AuthorizedPayments is removed in 6.0.0.")]
        public float? AuthorizedPayments
        {
            get { return _authorizedPayment; }
            set { _authorizedPayment = value; }
        }

        private float? _authorizedPayment;

        /// <summary>
        /// The authorized payment amount, sent as authorized_payment (the documented key) and
        /// authorized_payments. Same value as <see cref="AuthorizedPayments"/>.
        /// </summary>
        [JsonIgnore]
        public float? AuthorizedPayment
        {
            get { return _authorizedPayment; }
            set { _authorizedPayment = value; }
        }

        // Not sent with the compatibility switch on (5.7.x didn't send it).
        [JsonProperty(PropertyName = "authorized_payment")]
        private float? AuthorizedPaymentWire
        {
            get { return _legacyWireFormat ? null : _authorizedPayment; }
            set
            {
                if (value != null)
                {
                    _authorizedPayment = value;
                }
            }
        }

        [JsonProperty(PropertyName = "route_index")]
        public int? RouteIndex { get; set; }

        [JsonProperty(PropertyName = "leg_index")]
        public int? LegIndex { get; set; }

        /// <summary>
        /// The amount the customer is liable for if the vehicle is damaged.
        /// </summary>
        [JsonProperty(PropertyName = "damage_excess")]
        public float? DamageExcess { get; set; }
    }
}