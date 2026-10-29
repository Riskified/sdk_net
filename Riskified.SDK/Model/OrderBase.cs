using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Riskified.SDK.Model
{
    public abstract class OrderBase : AbstractOrder
    {

        public OrderBase(string merchantOrderId) : base(merchantOrderId)
        {
            
        }

        /// <summary>
        /// The customer's order name as represented by a number.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The session id that this order was created on, this value should match the session id value that is passed in the beacon JavaScript.
        /// </summary>
        [JsonProperty(PropertyName = "cart_token")]
        public string CartToken { get; set; }

        [JsonProperty(PropertyName = "device_id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// The date and time when the order was closed.
        /// </summary>
        [JsonProperty(PropertyName = "closed_at")]
        public DateTimeOffset? ClosedAt { get; set; }

        /// <summary>
        /// The date and time when the order was first created.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The three letter code (ISO 4217) for the currency used for the payment.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The customer's email address.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The payment gateway used.
        /// </summary>
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        /// <summary>
        /// The total amount of the discounts to be applied to the price of the order.
        /// </summary>
        [JsonProperty(PropertyName = "total_discounts")]
        public double? TotalDiscounts { get; set; }

        /// <summary>
        /// The sum of all the prices of all the items in the order, taxes and discounts included (must be positive).
        /// </summary>
        [JsonProperty(PropertyName = "total_price")]
        public double? TotalPrice { get; set; }

        [JsonProperty(PropertyName = "total_price_usd")]
        public double? TotalPriceUsd { get; set; }

        /// <summary>
        /// The date and time when the order was last modified.
        /// </summary>
        [JsonProperty(PropertyName = "updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// The IP address of the browser used by the customer when placing the order.
        /// </summary>
        [JsonProperty(PropertyName = "browser_ip")]
        public string CustomerBrowserIp { get; set; }

        /// <summary>
        /// A list of discount code objects, each one containing information about an item in the order.
        /// </summary>
        [JsonProperty(PropertyName = "discount_codes")]
        public DiscountCode[] DiscountCodes { get; set; }

        /// <summary>
        /// A list of line item objects, each one containing information about an item in the order.
        /// </summary>
        [JsonProperty(PropertyName = "line_items")]
        public LineItem[] LineItems { get; set; }

        /// <summary>
        /// A list of shipping line objects, each of which details the shipping methods used.
        /// </summary>
        [JsonProperty(PropertyName = "shipping_lines")]
        public ShippingLine[] ShippingLines { get; set; }

        /// <summary>
        /// An object containing information about the payment.
        /// </summary>
        [JsonProperty(PropertyName = "payment_details")]
        public IPaymentDetails[] PaymentDetails { get; set; }

        // billing_address / shipping_address are documented as arrays. The single and array
        // properties share one list each, and the private *Wire properties below send it.
        private readonly List<AddressInformation> _billingAddresses = new List<AddressInformation>();
        private readonly List<AddressInformation> _shippingAddresses = new List<AddressInformation>();
        private bool _legacyWireFormat;

        /// <summary>
        /// The mailing address associated with the payment method: the first of <see cref="BillingAddresses"/>.
        /// Setting it replaces the first billing address; null removes it.
        /// </summary>
        [JsonIgnore]
        public AddressInformation BillingAddress
        {
            get { return SingleOrArray.First(_billingAddresses); }
            set { SingleOrArray.SetFirst(_billingAddresses, value); }
        }

        /// <summary>
        /// The mailing addresses associated with the payment methods. The getter returns a copy;
        /// assign a new array to change them.
        /// </summary>
        [JsonIgnore]
        public AddressInformation[] BillingAddresses
        {
            get { return SingleOrArray.ToArray(_billingAddresses); }
            set { SingleOrArray.ReplaceAll(_billingAddresses, value); }
        }

        /// <summary>
        /// The mailing address to where the order will be shipped: the first of <see cref="ShippingAddresses"/>.
        /// Setting it replaces the first shipping address; null removes it.
        /// </summary>
        [JsonIgnore]
        public AddressInformation ShippingAddress
        {
            get { return SingleOrArray.First(_shippingAddresses); }
            set { SingleOrArray.SetFirst(_shippingAddresses, value); }
        }

        /// <summary>
        /// The mailing addresses to where the order will be shipped. The getter returns a copy;
        /// assign a new array to change them.
        /// </summary>
        [JsonIgnore]
        public AddressInformation[] ShippingAddresses
        {
            get { return SingleOrArray.ToArray(_shippingAddresses); }
            set { SingleOrArray.ReplaceAll(_shippingAddresses, value); }
        }

        [JsonProperty(PropertyName = "billing_address")]
        private object BillingAddressWire
        {
            get { return SingleOrArray.ToWire(_billingAddresses, SendsSingleAddresses); }
            set { SingleOrArray.FromWire(_billingAddresses, value); }
        }

        [JsonProperty(PropertyName = "shipping_address")]
        private object ShippingAddressWire
        {
            get { return SingleOrArray.ToWire(_shippingAddresses, SendsSingleAddresses); }
            set { SingleOrArray.FromWire(_shippingAddresses, value); }
        }

        // /checkout_denied documents a single address object; every other order endpoint an array.
        // The compatibility switch sends the 5.7.x single object everywhere.
        private bool SendsSingleAddresses
        {
            get { return _legacyWireFormat || this is OrderCheckoutDenied; }
        }

        [OnSerializing]
        private void OnSerializingOrderBase(StreamingContext context)
        {
            _legacyWireFormat = WireOptions.IsLegacy(context);
        }

        /// <summary>
        /// Applicable only for Merchants from the travel industry.
        /// </summary>
        [JsonProperty(PropertyName = "passengers")]
        public Passenger[] Passengers { get; set; }

        /// <summary>
        /// An object containing information about the customer.
        /// </summary>
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "financial_status")]
        public string FinancialStatus { get; set; }

        [JsonProperty(PropertyName = "fulfillment_status")]
        public string FulfillmentStatus { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "nocharge_amount")]
        public NoChargeDetails NoChargeAmount { get; set; }

        [JsonProperty(PropertyName = "vendor_id")]
        public string VendorId { get; set; }

        [JsonProperty(PropertyName = "vendor_name")]
        public string VendorName { get; set; }

        [JsonProperty(PropertyName = "vendor_integration_type")]
        public string VendorIntegrationType { get; set; }

        [JsonProperty(PropertyName = "decision")]
        public DecisionDetails Decision { get; set; }

        [JsonProperty(PropertyName = "referring_site")]
        public string ReferringSite { get; set; }

        /// <summary>
        /// Identifies the AI agent that completed the checkout autonomously on behalf of the consumer.
        /// Populate only for fully autonomous agentic orders (the agent completes checkout end-to-end).
        /// For agent referrals where the consumer completes checkout manually, use ReferringSite instead.
        /// </summary>
        [JsonProperty(PropertyName = "ai_agent")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AiAgent? AiAgent { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "decision_timeout")]
        public int? DecisionTimeout { get; set; }

        [JsonProperty(PropertyName = "additional_emails")]
        public string[] AdditionalEmails { get; set; }

        [JsonProperty(PropertyName = "client_details")]
        public ClientDetails ClientDetails { get; set; }

        // charge_free_payment_details is documented as an array on every endpoint; same bridge as the addresses.
        private readonly List<ChargeFreePaymentDetails> _chargeFreePaymentDetails = new List<ChargeFreePaymentDetails>();

        /// <summary>
        /// A non-chargebackable part of the payment: the first of <see cref="ChargeFreePaymentDetailsList"/>.
        /// Setting it replaces the first one; null removes it.
        /// </summary>
        [JsonIgnore]
        public ChargeFreePaymentDetails ChargeFreePaymentDetails
        {
            get { return SingleOrArray.First(_chargeFreePaymentDetails); }
            set { SingleOrArray.SetFirst(_chargeFreePaymentDetails, value); }
        }

        /// <summary>
        /// The non-chargebackable parts of the payment (e.g. gift card sums). The getter returns a copy;
        /// assign a new array to change them.
        /// </summary>
        [JsonIgnore]
        public ChargeFreePaymentDetails[] ChargeFreePaymentDetailsList
        {
            get { return SingleOrArray.ToArray(_chargeFreePaymentDetails); }
            set { SingleOrArray.ReplaceAll(_chargeFreePaymentDetails, value); }
        }

        // The compatibility switch sends the 5.7.x single object.
        [JsonProperty(PropertyName = "charge_free_payment_details")]
        private object ChargeFreePaymentDetailsWire
        {
            get { return SingleOrArray.ToWire(_chargeFreePaymentDetails, _legacyWireFormat); }
            set { SingleOrArray.FromWire(_chargeFreePaymentDetails, value); }
        }

        [JsonProperty(PropertyName = "group_founder_order_id")]
        public string GroupFounderOrderID { get; set; }

        [JsonProperty(PropertyName = "order_type")]
        public string OrderType { get; set; }
        
        [JsonProperty(PropertyName = "custom")]
        public Custom Custom { get; set; }

        [JsonProperty(PropertyName = "partner_sub_merchant_id")]
        public string PartnerSubMerchantId { get; set; } = null;

        /// <summary>
        /// The ID of the invoice associated with the order.
        /// </summary>
        [JsonProperty(PropertyName = "invoice_id")]
        public string InvoiceId { get; set; }

    }
}
