using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// The date and time when the order was closed.
        /// </summary>
        [JsonProperty(PropertyName = "closed_at")]
        public DateTime? ClosedAt { get; set; }

        /// <summary>
        /// The date and time when the order was first created.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

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
        public DateTime? UpdatedAt { get; set; }

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
        public IPaymentDetails PaymentDetails { get; set; }

        /// <summary>
        /// The mailing address associated with the payment method.
        /// </summary>
        [JsonProperty(PropertyName = "billing_address")]
        public AddressInformation BillingAddress { get; set; }

        /// <summary>
        /// The mailing address to where the order will be shipped.
        /// </summary>
        [JsonProperty(PropertyName = "shipping_address")]
        public AddressInformation ShippingAddress { get; set; }

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

        [JsonProperty(PropertyName = "decision")]
        public DecisionDetails Decision { get; set; }

        [JsonProperty(PropertyName = "referring_site")]
        public string ReferringSite { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "decision_timeout")]
        public int? DecisionTimeout { get; set; }

        [JsonProperty(PropertyName = "additional_emails")]
        public string[] AdditionalEmails { get; set; }

        [JsonProperty(PropertyName = "client_details")]
        public ClientDetails ClientDetails { get; set; }

        [JsonProperty(PropertyName = "charge_free_payment_details")]
        public ChargeFreePaymentDetails ChargeFreePaymentDetails { get; set; } 

        [JsonProperty(PropertyName = "group_founder_order_id")]
        public string GroupFounderOrderID { get; set; }

        [JsonProperty(PropertyName = "order_type")]
        public string OrderType { get; set; }
    }
}
