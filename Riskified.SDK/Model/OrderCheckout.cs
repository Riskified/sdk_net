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
    public class OrderCheckout : OrderBase
    {
        /// <summary>
        /// Creates a new order checkout
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        public OrderCheckout(int merchantOrderId) : base(merchantOrderId)
        {

        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);

            // All properties are optional, so we only validated when they're filled.
            
            if(LineItems != null)
            {
                LineItems.ToList().ForEach(item => item.Validate(validationType));
            }

            
            if(ShippingAddress != null)
            {
                ShippingLines.ToList().ForEach(item => item.Validate(validationType));
            }
            
            if (PaymentDetails != null)
            {
                PaymentDetails.Validate(validationType);
            }

            if(NoChargeAmount != null)
            {
                NoChargeAmount.Validate(validationType);
            }

            if (validationType == Validations.Weak)
            {
                if (BillingAddress != null)
                {
                    BillingAddress.Validate(validationType);
                }
                else if(ShippingAddress != null)
                {
                    ShippingAddress.Validate(validationType);
                }
            }
            else
            {
                if(BillingAddress != null)
                {
                    BillingAddress.Validate(validationType);
                }
                if(ShippingAddress != null)
                {
                    ShippingAddress.Validate(validationType);
                }
            }

            if(Customer != null)
            {
                Customer.Validate(validationType);
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
                DiscountCodes.ToList().ForEach(item => item.Validate(validationType));
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

            if (Decision != null)
            {
                Decision.Validate(validationType);
            }

        }

    }
}
