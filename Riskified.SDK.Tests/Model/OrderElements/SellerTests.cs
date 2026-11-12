using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    /// <summary>
    /// Step 3.8: seller.starting_price is sent as a float-formatted string, as the API documents it.
    /// </summary>
    public class SellerTests
    {
        private static JToken StartingPriceSent(float? startingPrice, bool legacy = false)
        {
            var seller = new Seller(new Customer("John", "Doe", "1"), startingPrice: startingPrice);
            var settings = HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null);
            return JObject.Parse(JsonConvert.SerializeObject(seller, settings))["starting_price"]!;
        }

        [Theory]
        [InlineData(120f, "120.00")]
        [InlineData(99.5f, "99.50")]
        [InlineData(0f, "0.00")]
        [InlineData(12.345f, "12.345")]
        public void Serialize_SendsAFloatFormattedString(float startingPrice, string expected)
        {
            var sent = StartingPriceSent(startingPrice);

            Assert.Equal(JTokenType.String, sent.Type);
            Assert.Equal(expected, sent.Value<string>());
        }

        [Fact]
        public void Serialize_IgnoresTheThreadCulture()
        {
            var original = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

                Assert.Equal("120.50", StartingPriceSent(120.5f).Value<string>());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        [Fact]
        public void Serialize_WithLegacyWireFormat_SendsTheNumber()
        {
            var sent = StartingPriceSent(120f, legacy: true);

            Assert.Equal(JTokenType.Float, sent.Type);
            Assert.Equal(120, sent.Value<double>());
        }

        [Fact]
        public void Serialize_Unset_SendsNothing()
        {
            var seller = new Seller(new Customer("John", "Doe", "1"));

            Assert.Null(JObject.Parse(JsonConvert.SerializeObject(seller, HttpUtils.CreateSerializerSettings()))["starting_price"]);
        }

        [Theory]
        [InlineData("{\"starting_price\": \"120.50\"}")]
        [InlineData("{\"starting_price\": 120.5}")]
        public void Deserialize_StringOrNumber_SetsStartingPrice(string json)
        {
            var seller = new Seller(new Customer("John", "Doe", "1"));

            JsonConvert.PopulateObject(json, seller);

            Assert.Equal(120.5f, seller.StartingPrice);
        }
    }
}
