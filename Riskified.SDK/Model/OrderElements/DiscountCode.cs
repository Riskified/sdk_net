using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class DiscountCode : IJsonSerializable
    {
        /// <summary>
        /// A discount code for one of the products in the order
        /// </summary>
        /// <param name="moneyDiscountSum">The amount of money (in the currency specified in the order) that was discounted (optional) </param>
        /// <param name="code">The discount code (optional) </param>
        public DiscountCode(double? moneyDiscountSum = null, string code = null)
        {
            MoneyDiscountSum = moneyDiscountSum;
            Code = code;
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

        [JsonProperty(PropertyName = "amount")]
        public double? MoneyDiscountSum { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        
    }

}
