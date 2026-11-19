using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class BasicAddressTests
    {
        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, HttpUtils.CreateSerializerSettings()));
        }

        [Fact]
        public void Serialize_WithNewPropertiesUnset_SendsNoneOfThem()
        {
            var json = Serialize(new BasicAddress(address1: "27 5th avenue", city: "Manhattan"));

            Assert.All(new[] { "id", "neighborhood", "latitude", "longitude" }, key => Assert.Null(json[key]));
        }

        [Fact]
        public void Serialize_KeepsFullCoordinatePrecision()
        {
            var json = Serialize(new BasicAddress { Latitude = -33.8567844, Longitude = 151.2152967 });

            Assert.Equal(-33.8567844, json["latitude"]!.Value<double>());
            Assert.Equal(151.2152967, json["longitude"]!.Value<double>());
        }
    }
}
