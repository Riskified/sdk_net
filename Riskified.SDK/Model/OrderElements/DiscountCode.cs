using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class DiscountCode
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

        [JsonProperty(PropertyName = "amount", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public double? MoneyDiscountSum { get; set; }

        [JsonProperty(PropertyName = "code", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

}
