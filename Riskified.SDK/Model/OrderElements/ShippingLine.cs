using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{

    public class ShippingLine : IJsonSerializable
    {
        /// <summary>
        /// The shipping line (shiiping method)
        /// </summary>
        /// <param name="price">The price of this shipping method</param>
        /// <param name="title">A human readable name for the shipping method</param>
        /// <param name="code">A code to the shipping method</param>
        public ShippingLine(double price, string title, string code = null)
        {
            Price = price;
            Title = title;
            // optional
            Code = code;
        }

        public void Validate(bool isWeak = false)
        {
            InputValidators.ValidateZeroOrPositiveValue(Price.Value, "Price");
            InputValidators.ValidateValuedString(Title, "Title");
        }

        [JsonProperty(PropertyName = "code", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "price", Required = Required.Always)]
        public double? Price { get; set; }

        [JsonProperty(PropertyName = "title", Required = Required.Always)]
        public string Title { get; set; }
    }

}
