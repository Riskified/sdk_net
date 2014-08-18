using System;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.RefundElements
{
    public class RefundDetails
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundedAt"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="reason"></param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public RefundDetails(DateTime refundedAt, double amount, string currency, string reason)
        {
            InputValidators.ValidateDateNotDefault(refundedAt, "Refunded At");
            RefundedAt = refundedAt;
            InputValidators.ValidateZeroOrPositiveValue(amount, "Refund Amount");
            Amount = amount;
            InputValidators.ValidateCurrency(currency);
            Currency = currency;
            InputValidators.ValidateValuedString(reason,"Refund Reason");
            Reason = reason;
        }

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
