using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class ChargeFreePaymentDetails : IJsonSerializable
    {
        /// <summary>
        /// Creates a new Non-chargebackable payment sum
        /// </summary>
        /// <param name="gateway">The gateway used to pay this payment part</param>
        /// <param name="amount">The sum payed using this method</param>
        public ChargeFreePaymentDetails(string gateway, double amount)
        {
            Gateway = gateway;
            Amount = amount;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateValuedString(Gateway, "Gateway");
            InputValidators.ValidateZeroOrPositiveValue(Amount, "Amount");
        }

        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }
    }
}
