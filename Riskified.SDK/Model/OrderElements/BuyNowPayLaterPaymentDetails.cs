using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// Payment information for buy now, pay later payments. Maps to the API's
    /// buy_now_pay_later_payment_details schema.
    /// </summary>
    public class BuyNowPayLaterPaymentDetails : IPaymentDetails
    {
        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "payment_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentType PaymentType { get; } = PaymentType.Bnpl;

        /// <summary>
        /// The amount of the first installment.
        /// </summary>
        [JsonProperty(PropertyName = "initial_payment_amount")]
        public double? InitialPaymentAmount { get; set; }

        /// <summary>
        /// The number of installments.
        /// </summary>
        [JsonProperty(PropertyName = "installments")]
        public int? Installments { get; set; }

        /// <summary>
        /// The number of days between installments.
        /// </summary>
        [JsonProperty(PropertyName = "payment_frequency")]
        public int? PaymentFrequency { get; set; }
    }
}
