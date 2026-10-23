using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Internal;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    /// <summary>
    /// Step 3.3: pickup_address / dropoff_address are sent as arrays, bridged from the existing single
    /// PickupAddress / DropoffAddress properties. The list rules are covered by OrderBaseAddressesTests.
    /// </summary>
    public class RideTicketLineItemAddressesTests
    {
        private static BasicAddress Address(string city)
        {
            return new BasicAddress(address1: "1 Main street", city: city, countryCode: "US");
        }

        private static JsonSerializerSettings Settings(bool legacy)
        {
            return HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null);
        }

        [Fact]
        public void Serialize_ConstructorAddresses_SendArrays()
        {
            var ride = new RideTicketLineItem("Ride", 74, 1, pickupAddress: Address("Queens"), dropoffAddress: Address("Manhattan"));

            var json = JObject.Parse(JsonConvert.SerializeObject(ride, Settings(legacy: false)));

            Assert.Equal("Queens", Assert.Single((JArray)json["pickup_address"]!)["city"]!.Value<string>());
            Assert.Equal("Manhattan", Assert.Single((JArray)json["dropoff_address"]!)["city"]!.Value<string>());
        }

        [Fact]
        public void Serialize_SeveralAddresses_SendsAllOfThem()
        {
            var ride = new RideTicketLineItem("Ride", 74, 1) { PickupAddresses = new[] { Address("Queens"), Address("Brooklyn") } };

            var pickup = (JArray)JObject.Parse(JsonConvert.SerializeObject(ride, Settings(legacy: false)))["pickup_address"]!;

            Assert.Equal(new[] { "Queens", "Brooklyn" }, pickup.Select(a => a["city"]!.Value<string>()));
        }

        [Fact]
        public void Serialize_InsideAnOrder_WithLegacyWireFormat_SendsObjects()
        {
            var ride = new RideTicketLineItem("Ride", 74, 1, pickupAddress: Address("Queens"), dropoffAddress: Address("Manhattan"));
            var wrapper = new OrderWrapper<AbstractOrder>(new OrderCheckout("1") { LineItems = new LineItem[] { ride } });

            var json = JObject.Parse(JsonConvert.SerializeObject(wrapper, Settings(legacy: true)));
            var lineItem = json["order"]!["line_items"]![0]!;

            Assert.Equal(JTokenType.Object, lineItem["pickup_address"]!.Type);
            Assert.Equal("Manhattan", lineItem["dropoff_address"]!["city"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutAddresses_SendsNothing()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new RideTicketLineItem("Ride", 74, 1), Settings(legacy: false)));

            Assert.Null(json["pickup_address"]);
            Assert.Null(json["dropoff_address"]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_EitherShape_RoundTrips(bool legacy)
        {
            var original = new RideTicketLineItem("Ride", 74, 1, pickupAddress: Address("Queens"), dropoffAddress: Address("Manhattan"));
            var json = JsonConvert.SerializeObject(original, Settings(legacy));

            var copy = new RideTicketLineItem("Ride", 74, 1);
            JsonConvert.PopulateObject(json, copy);

            Assert.Equal("Queens", copy.PickupAddress.City);
            Assert.Equal("Manhattan", copy.DropoffAddress.City);
        }
    }
}
