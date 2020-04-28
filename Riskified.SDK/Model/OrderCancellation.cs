using System;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderCancellation : AbstractOrder
    {
        /// <summary>
        /// Creates an order cancellation 
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="cancelledAt">The date and time when the order was cancelled (optional)</param>
        /// <param name="cancelReason">If the order was cancelled, the value will be one of the following:
        /// "customer": The customer changed or cancelled the order.
        /// "fraud": The order was fraudulent.
        /// "inventory": Items in the order were not in inventory.
        /// "other": The order was cancelled for a reason not in the list above. </param>
        public OrderCancellation(int merchantOrderId, DateTimeOffset? cancelledAt, string cancelReason) : base(merchantOrderId)
        {
            CancelledAt = cancelledAt;
            CancelReason = cancelReason;
        }

        /// <summary>
        /// Creates an order cancellation
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="cancelledAt">The date and time when the order was cancelled (optional)</param>
        /// <param name="cancelReason">If the order was cancelled, the value will be one of the following:
        /// "customer": The customer changed or cancelled the order.
        /// "fraud": The order was fraudulent.
        /// "inventory": Items in the order were not in inventory.
        /// "other": The order was cancelled for a reason not in the list above. </param>
        public OrderCancellation(string merchantOrderId, DateTimeOffset? cancelledAt, string cancelReason) : base(merchantOrderId)
        {
            CancelledAt = cancelledAt;
            CancelReason = cancelReason;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
            InputValidators.ValidateDateNotDefault(CancelledAt.Value, "Cancelled At");
            InputValidators.ValidateValuedString(CancelReason, "Cancel Reason");
        }


        [JsonProperty(PropertyName = "cancel_reason")]
        public string CancelReason { get; set; }

        [JsonProperty(PropertyName = "cancelled_at")]
        public DateTimeOffset? CancelledAt { get; set; }
    }
}
