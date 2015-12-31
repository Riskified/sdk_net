using System;
using System.Linq;
using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    
    public class Order : OrderBase
    {
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="email">The email used for contact in the order</param>
        /// <param name="customer">The customer information</param>
        /// <param name="paymentDetails">The payment details</param>
        /// <param name="billingAddress">Billing address</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="lineItems">An array of all products in the order</param>
        /// <param name="shippingLines">An array of all shipping details for the order</param>
        /// <param name="gateway">The payment gateway that was used</param>
        /// <param name="customerBrowserIp">The customer browser ip that was used for the order</param>
        /// <param name="currency">A three letter code (ISO 4217) for the currency used for the payment</param>
        /// <param name="totalPrice">The sum of all the prices of all the items in the order, taxes and discounts included</param>
        /// <param name="createdAt">The date and time when the order was created</param>
        /// <param name="updatedAt">The date and time when the order was last modified</param>
        /// <param name="discountCodes">An array of objects, each one containing information about an item in the order (optional)</param>
        /// <param name="totalDiscounts">The total amount of the discounts on the Order (optional)</param>
        /// <param name="cartToken">Unique identifier for a particular cart or session that is attached to a particular order. The same ID should be passed in the Beacon JS (optional)</param>
        /// <param name="totalPriceUsd">The price in USD (optional)</param>
        /// <param name="closedAt">The date and time when the order was closed. If the order was closed (optional)</param>
        /// <param name="financialStatus">The financial status of the order (could be paid/voided/refunded/partly_paid/etc.)</param>
        /// <param name="fulfillmentStatus">The fulfillment status of the order</param>
        /// <param name="source">The source of the order</param>
        /// <param name="noChargeDetails">No charge sums - including all payments made for this order in giftcards, cash, checks or other non chargebackable payment methods</param>
        /// <param name="ClientDetails">Technical information regarding the customer's browsing session</param>
        /// <param name="chargeFreePaymentDetails">Payment sums made using non-chargebackable methods and should be omitted from the Chargeback gurantee sum and Riskified fee</param>
        public Order(string merchantOrderId,
                     string email, 
                     Customer customer,
                     AddressInformation billingAddress, 
                     AddressInformation shippingAddress, 
                     LineItem[] lineItems,
                     ShippingLine[] shippingLines,
                     string gateway, 
                     string customerBrowserIp, 
                     string currency, 
                     double totalPrice, 
                     DateTime createdAt,
                     DateTime updatedAt,
                     IPaymentDetails paymentDetails = null, 
                     DiscountCode[] discountCodes = null, 
                     double? totalDiscounts = null, 
                     string cartToken = null, 
                     double? totalPriceUsd = null, 
                     DateTime? closedAt = null,
                     string financialStatus = null,
                     string fulfillmentStatus = null,
                     string source = null, 
                     NoChargeDetails noChargeDetails = null,
                     string[] additionalEmails = null,
                     string vendorId = null,
                     string vendorName = null,
                     DecisionDetails decisionDetails = null,
                     ClientDetails clientDetails = null,
                     ChargeFreePaymentDetails chargeFreePaymentDetails = null,
                     string groupFounderOrderID = null) : base(merchantOrderId)
        {
            LineItems = lineItems;
            ShippingLines = shippingLines;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            Customer = customer;
            Email = email;
            CustomerBrowserIp = customerBrowserIp;
            Currency = currency;
            TotalPrice = totalPrice;
            Gateway = gateway;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            
            // optional fields
            PaymentDetails = paymentDetails;
            DiscountCodes = discountCodes;
            TotalPriceUsd = totalPriceUsd;
            TotalDiscounts = totalDiscounts;
            CartToken = cartToken;
            ClosedAt = closedAt;
            FinancialStatus = financialStatus;
            FulfillmentStatus = fulfillmentStatus;
            Source = source;
            NoChargeAmount = noChargeDetails;
            AdditionalEmails = additionalEmails;
            VendorId = vendorId;
            VendorName = vendorName;
            Decision = decisionDetails;
            ClientDetails = clientDetails;
            ChargeFreePaymentDetails = chargeFreePaymentDetails;

            // This field is added for gift card group purchase
            GroupFounderOrderID = groupFounderOrderID;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
            InputValidators.ValidateObjectNotNull(LineItems, "Line Items");
            LineItems.ToList().ForEach(item => item.Validate(validationType));
            InputValidators.ValidateObjectNotNull(ShippingLines, "Shipping Lines");
            ShippingLines.ToList().ForEach(item => item.Validate(validationType));
            if(PaymentDetails == null && NoChargeAmount == null)
            {
                throw new Exceptions.OrderFieldBadFormatException("Both PaymentDetails and NoChargeDetails are missing - at least one should be specified");
            }
            if(PaymentDetails != null)
            {
                PaymentDetails.Validate(validationType);
            }
            else 
            {
                NoChargeAmount.Validate(validationType);
            }

            if (validationType == Validations.Weak)
            {
                if (BillingAddress == null && ShippingAddress == null)
                {
                    throw new Exceptions.OrderFieldBadFormatException("Both shipping and billing addresses are missing - at least one should be specified");
                }

                if (BillingAddress != null)
                {
                    BillingAddress.Validate(validationType);
                }
                else
                {
                    ShippingAddress.Validate(validationType);
                }
            }
            else
            {
                InputValidators.ValidateObjectNotNull(BillingAddress, "Billing Address");
                BillingAddress.Validate(validationType);
                InputValidators.ValidateObjectNotNull(ShippingAddress, "Shipping Address");
                ShippingAddress.Validate(validationType);
            }

            InputValidators.ValidateObjectNotNull(Customer, "Customer");
            Customer.Validate(validationType);
            InputValidators.ValidateEmail(Email);
            InputValidators.ValidateIp(CustomerBrowserIp);
            InputValidators.ValidateCurrency(Currency);
            InputValidators.ValidateZeroOrPositiveValue(TotalPrice.Value, "Total Price");
            InputValidators.ValidateValuedString(Gateway, "Gateway");
            InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            InputValidators.ValidateDateNotDefault(UpdatedAt.Value, "Updated At");
            
            // optional fields validations
            if(DiscountCodes != null && DiscountCodes.Length > 0)
            {
                DiscountCodes.ToList().ForEach(item => item.Validate(validationType));
            }
            if(TotalPriceUsd.HasValue)
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
        /// The unique identifier of the Checkout that created this order.
        /// </summary>
        [JsonProperty(PropertyName = "checkout_id")]
        public string CheckoutId { get; set; }
        
    }

}
