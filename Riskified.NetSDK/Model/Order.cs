using System;
using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{

    public class Order
    {
        
        private string _email;
        //tODO consider adding empty strings validations or invalid numbers (negative etc..)
        [JsonProperty(PropertyName = "cancel_reason", Required = Required.Default)]
        public string CancelReason { get; set; }

        [JsonProperty(PropertyName = "cancelled_at", Required = Required.Default)]
        public DateTime? OrderCancellationTime { get; set; }

        [JsonProperty(PropertyName = "cart_token", Required = Required.Default)]
        public string CartToken { get; set; }

        [JsonProperty(PropertyName = "closed_at", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "currency", Required = Required.Always)]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public string Email
        {
            get { return _email; }
            set
            {
                InputValidators.ValidateEmail(value);
                _email = value;
            }
        }

        [JsonProperty(PropertyName = "gateway", Required = Required.Always)]
        public string Gateway { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "total_discounts", Required = Required.Default)]
        public double? TotalDiscounts { get; set; }

        [JsonProperty(PropertyName = "total_price", Required = Required.Always)]
        public double? TotalPrice { get; set; }

        [JsonProperty(PropertyName = "total_price_usd",Required = Required.Default)]
        public double? TotalPriceUsd { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Always)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "browser_ip", Required = Required.Always)]
        public string CustomerIp { get; set; }
        
        [JsonProperty(PropertyName = "discount_codes", Required = Required.Default)]
        public DiscountCode[] DiscountCodes { get; set; }

        [JsonProperty(PropertyName = "line_items", Required = Required.Always)]
        public LineItem[] LineItems { get; set; }

        [JsonProperty(PropertyName = "shipping_lines", Required = Required.Always)]
        public ShippingLine[] ShippingLines { get; set; }

        [JsonProperty(PropertyName = "payment_details", Required = Required.Always)]
        public PaymentDetails PaymentDetails { get; set; }

        [JsonProperty(PropertyName = "billing_address", Required = Required.Always)]
        public AddressInformation BillingAddress { get; set; }

        [JsonProperty(PropertyName = "shipping_address", Required = Required.Always)]
        public AddressInformation ShippingAddress { get; set; }

        [JsonProperty(PropertyName = "customer", Required = Required.Always)]
        public Customer Customer { get; set; }
    }

}
