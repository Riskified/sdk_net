using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    public class OrderCheckout : AbstractOrder
    {
        /// <summary>
        /// Creates a new order checkout
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        public OrderCheckout(int merchantOrderId) : base(merchantOrderId)
        {

        }

        public override void Validate(bool isWeak = false)
        {
            base.Validate(isWeak);

            // All properties are optional, so we only validated when they're filled.
            
            if(LineItems != null)
            {
                LineItems.ToList().ForEach(item => item.Validate(isWeak));
            }

            
            if(ShippingAddress != null)
            {
                ShippingLines.ToList().ForEach(item => item.Validate(isWeak));
            }
            
            if (PaymentDetails != null)
            {
                PaymentDetails.Validate(isWeak);
            }

            if(NoChargeAmount != null)
            {
                NoChargeAmount.Validate(isWeak);
            }

            if (isWeak)
            {
                if (BillingAddress != null)
                {
                    BillingAddress.Validate(isWeak);
                }
                else if(ShippingAddress != null)
                {
                    ShippingAddress.Validate(isWeak);
                }
            }
            else
            {
                if(BillingAddress != null)
                {
                    BillingAddress.Validate(isWeak);
                }
                if(ShippingAddress != null)
                {
                    ShippingAddress.Validate(isWeak);
                }
            }

            if(Customer != null)
            {
                Customer.Validate(isWeak);
            }
            if(!string.IsNullOrEmpty(Email))
            {
                InputValidators.ValidateEmail(Email);
            }
            if (!string.IsNullOrEmpty(CustomerBrowserIp))
            {
                InputValidators.ValidateIp(CustomerBrowserIp);
            }
            if (!string.IsNullOrEmpty(Currency))
            {
                InputValidators.ValidateCurrency(Currency);
            }
            if (TotalPrice.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalPrice.Value, "Total Price");
            }
           
            if(CreatedAt != null)
            {
                InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            }
            if (UpdatedAt != null)
            {
                InputValidators.ValidateDateNotDefault(UpdatedAt.Value, "Updated At");
            }

            if (DiscountCodes != null && DiscountCodes.Length > 0)
            {
                DiscountCodes.ToList().ForEach(item => item.Validate(isWeak));
            }
            if (TotalPriceUsd.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalPriceUsd.Value, "Total Price USD");
            }
            if (TotalDiscounts.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalDiscounts.Value, "Total Discounts");
            }
            if (ClosedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(ClosedAt.Value, "Closed At");
            }

        }

        /// <summary>
        /// Overrides order object fields with an order checkout object fields (non null fields).
        /// </summary>
        /// <param name="orderCheckout">an order checkout object that his fields will be assign to the current order fields.</param>
        public void ImportOrderCheckout(OrderCheckout orderCheckout)
        {
            if (!string.IsNullOrEmpty(orderCheckout.CartToken))
            {
                this.CartToken = orderCheckout.CartToken;
            }

            if (orderCheckout.LineItems != null)
            {
                this.LineItems = orderCheckout.LineItems;
            }

            if (orderCheckout.ShippingAddress != null)
            {
                this.ShippingAddress = orderCheckout.ShippingAddress;
            }

            if (orderCheckout.PaymentDetails != null)
            {
                this.PaymentDetails = orderCheckout.PaymentDetails;
            }

            if (orderCheckout.NoChargeAmount != null)
            {
                this.NoChargeAmount = orderCheckout.NoChargeAmount;
            }

            if (orderCheckout.BillingAddress != null)
            {
                this.BillingAddress = orderCheckout.BillingAddress;
            }

            if (orderCheckout.ShippingAddress != null)
            {
                this.ShippingAddress = orderCheckout.ShippingAddress;
            }

            if (orderCheckout.Customer != null)
            {
                this.Customer = orderCheckout.Customer;
            }
            if (!string.IsNullOrEmpty(orderCheckout.Email))
            {
                this.Email = orderCheckout.Email;
            }
            if (!string.IsNullOrEmpty(orderCheckout.CustomerBrowserIp))
            {
                this.CustomerBrowserIp = orderCheckout.CustomerBrowserIp;
            }

            if (!string.IsNullOrEmpty(orderCheckout.Currency))
            {
                this.Currency = orderCheckout.Currency;
            }

            if (!string.IsNullOrEmpty(orderCheckout.Gateway))
            {
                this.Gateway = orderCheckout.Gateway;
            }

            if (!string.IsNullOrEmpty(orderCheckout.FinancialStatus))
            {
                this.FinancialStatus = orderCheckout.FinancialStatus;
            }
            if (orderCheckout.TotalPrice.HasValue)
            {
                this.TotalPrice = orderCheckout.TotalPrice;
            }
            if (orderCheckout.CreatedAt != null)
            {
                this.CreatedAt = orderCheckout.CreatedAt;
            }
            if (orderCheckout.UpdatedAt != null)
            {
                this.UpdatedAt = orderCheckout.UpdatedAt;
            }

            if (orderCheckout.DiscountCodes != null)
            {
                this.DiscountCodes = orderCheckout.DiscountCodes;
            }

            if (orderCheckout.TotalPriceUsd.HasValue)
            {
                this.TotalPriceUsd = orderCheckout.TotalPriceUsd;
            }

            if (orderCheckout.TotalDiscounts.HasValue)
            {
                this.TotalDiscounts = orderCheckout.TotalDiscounts;
            }

            if (orderCheckout.ClosedAt.HasValue)
            {
                this.ClosedAt = orderCheckout.ClosedAt;
            }


        }

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
    }
}
