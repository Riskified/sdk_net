using System;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.RefundElements
{
    public class PartialRefundDetails
    {
        /// <summary>
        /// The details for a single partial refund element
        /// </summary>
        /// <param name="refundId">A unique identifier for the refund record at the merchant's system</param>
        /// <param name="refundedAt">Date and time when the refund occured</param>
        /// <param name="amount">The amount refunded (at the specified currency)</param>
        /// <param name="currency">The currency of the refund amount</param>
        /// <param name="reason">The reason for the partial refund</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public PartialRefundDetails(string refundId,DateTime refundedAt, double amount, string currency, string reason)
        {
            InputValidators.ValidateValuedString(refundId,"Refund ID");
            RefundId = refundId;
            InputValidators.ValidateDateNotDefault(refundedAt, "Refunded At");
            RefundedAt = refundedAt;
            InputValidators.ValidateZeroOrPositiveValue(amount, "Refund Amount");
            Amount = amount;
            InputValidators.ValidateCurrency(currency);
            Currency = currency;
            InputValidators.ValidateValuedString(reason,"Refund Reason");
            Reason = reason;
        }

        [JsonProperty(PropertyName = "refund_id", Required = Required.Always)]
        public string RefundId { get; set; }

        [JsonProperty(PropertyName = "refunded_at", Required = Required.Always)]
        public DateTime RefundedAt { get; set; }

        [JsonProperty(PropertyName = "amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty(PropertyName = "currency", Required = Required.Always)]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "reason", Required = Required.Always)]
        public string Reason { get; set; }
    }
}
