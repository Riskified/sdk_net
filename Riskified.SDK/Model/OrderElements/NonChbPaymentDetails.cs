using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// Payment information for payment methods that can't be charged back (gift cards, store credit,
    /// reward points). Maps to the API's non_chb_payment_details schema.
    /// </summary>
    public class NonChbPaymentDetails : IPaymentDetails
    {
        /// <summary>
        /// Creates a non-chargeback payment details object.
        /// </summary>
        /// <param name="paymentType">The payment type. The documented values are gift_card, store_credit and rewards_points.</param>
        public NonChbPaymentDetails(PaymentType paymentType)
        {
            PaymentType = paymentType;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "payment_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentType PaymentType { get; set; }
    }
}
