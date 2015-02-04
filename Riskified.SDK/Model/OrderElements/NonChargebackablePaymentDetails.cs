using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class NonChargebackablePaymentDetails : IPaymentDetails
    {
        /// <summary>
        /// The payment information for the order in case of a non chargebackable payment method such as giftcards/check/cash orders & etc.
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <param name="notes"></param>
        public NonChargebackablePaymentDetails(string paymentMethod = null, string notes = null )
        {
            PaymentMethod = paymentMethod;
            Notes = notes;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(bool isWeak = false)
        {
            return;
        }

        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
