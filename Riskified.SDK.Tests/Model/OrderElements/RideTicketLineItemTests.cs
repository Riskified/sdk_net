using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class RideTicketLineItemTests
    {
        // Mirrors the production serialization settings from Riskified.SDK.Utils.HttpUtils.
        private static readonly JsonSerializerSettings SdkSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new StringEnumConverter() }
        };

        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, SdkSettings));
        }

        private static RideTicketLineItem RideWithGeolocation()
        {
            return new RideTicketLineItem(
                title: "Airport transfer",
                price: 42.5,
                quantityPurchased: 1,
                pickupLatitude: 32.0853f,
                pickupLongitude: 34.7818f,
                dropoffLatitude: 31.7683f,
                dropoffLongitude: 35.2137f);
        }

        /// <summary>
        /// Regression test for the wire name of the dropoff latitude.
        ///
        /// This property serialized as "dropoff_latitiude" -- a transposition -- from the
        /// field's introduction until this test was added. Both OpenAPI specs declare
        /// "dropoff_latitude", with a description and an example, and the transposed
        /// spelling appears in neither. The Java, PHP and JavaScript SDKs all derive the
        /// correct spelling, so this SDK was the only one sending the wrong key.
        ///
        /// Asserted in both directions so the transposition cannot come back.
        /// </summary>
        [Fact]
        public void Serialize_DropoffLatitude_UsesTheContractWireName()
        {
            var json = Serialize(RideWithGeolocation());

            Assert.Equal(31.7683f, (float)json["dropoff_latitude"]);
            Assert.Null(json["dropoff_latitiude"]);
        }

        [Fact]
        public void Serialize_RideGeolocationFields_UseTheirContractWireNames()
        {
            var json = Serialize(RideWithGeolocation());

            Assert.Equal(32.0853f, (float)json["pickup_latitude"]);
            Assert.Equal(34.7818f, (float)json["pickup_longitude"]);
            Assert.Equal(31.7683f, (float)json["dropoff_latitude"]);
            Assert.Equal(35.2137f, (float)json["dropoff_longitude"]);
        }
    }
}
