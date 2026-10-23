using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model
{
    /// <summary>
    /// Step 3.1: billing_address / shipping_address are sent as arrays, bridged from the existing single
    /// BillingAddress / ShippingAddress properties.
    /// </summary>
    public class OrderBaseAddressesTests
    {
        private static AddressInformation Address(string city)
        {
            return new AddressInformation("Ben", "Rolling", "27 5th avenue", city, "United States", "US", "5554321234");
        }

        private static JObject Serialize(object obj, bool legacy = false)
        {
            var settings = HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null);
            return JObject.Parse(JsonConvert.SerializeObject(obj, settings));
        }

        [Fact]
        public void SingleSetter_OnEmptyList_AddsTheAddress()
        {
            var order = new OrderCheckout("1") { BillingAddress = Address("Manhattan") };

            Assert.Equal("Manhattan", Assert.Single(order.BillingAddresses).City);
        }

        [Fact]
        public void SingleSetter_ReplacesOnlyTheFirstAddress()
        {
            var order = new OrderCheckout("1") { BillingAddresses = new[] { Address("Manhattan"), Address("Brooklyn") } };

            order.BillingAddress = Address("Queens");

            Assert.Equal(new[] { "Queens", "Brooklyn" }, order.BillingAddresses.Select(a => a.City));
        }

        [Fact]
        public void SingleSetter_Null_RemovesTheFirstAddress()
        {
            var order = new OrderCheckout("1") { BillingAddresses = new[] { Address("Manhattan"), Address("Brooklyn") } };

            order.BillingAddress = null;

            Assert.Equal("Brooklyn", order.BillingAddress.City);
            Assert.Single(order.BillingAddresses);
        }

        [Fact]
        public void SingleSetter_Null_OnOneAddress_SendsNothing()
        {
            var order = new OrderCheckout("1") { BillingAddress = Address("Manhattan") };

            order.BillingAddress = null;

            Assert.Null(order.BillingAddresses);
            Assert.Null(Serialize(order)["billing_address"]);
        }

        [Fact]
        public void ArraySetter_CopiesTheCallersArray()
        {
            var addresses = new[] { Address("Manhattan") };
            var order = new OrderCheckout("1") { BillingAddresses = addresses };

            addresses[0] = Address("Brooklyn");
            order.BillingAddress = Address("Queens");

            Assert.Equal("Brooklyn", addresses[0].City);
            Assert.Equal("Queens", order.BillingAddress.City);
        }

        [Fact]
        public void ArrayGetter_ReturnsACopy()
        {
            var order = new OrderCheckout("1") { BillingAddress = Address("Manhattan") };

            order.BillingAddresses[0] = Address("Brooklyn");

            Assert.Equal("Manhattan", order.BillingAddress.City);
        }

        [Fact]
        public void EmptyOrNullArray_SendsNothing()
        {
            var empty = new OrderCheckout("1") { BillingAddresses = new AddressInformation[0], ShippingAddresses = null };

            var json = Serialize(empty);

            Assert.Null(json["billing_address"]);
            Assert.Null(json["shipping_address"]);
        }

        [Fact]
        public void Serialize_OrderAndCheckout_SendArrays()
        {
            var checkout = new OrderCheckout("1") { BillingAddresses = new[] { Address("Manhattan"), Address("Brooklyn") }, ShippingAddress = Address("Queens") };

            var json = Serialize(checkout);

            Assert.Equal(new[] { "Manhattan", "Brooklyn" }, ((JArray)json["billing_address"]!).Select(a => a["city"]!.Value<string>()));
            Assert.Equal("Queens", Assert.Single((JArray)json["shipping_address"]!)["city"]!.Value<string>());
        }

        [Fact]
        public void Serialize_CheckoutDenied_SendsTheFirstAddressAsAnObject()
        {
            var denied = new OrderCheckoutDenied("1") { BillingAddresses = new[] { Address("Manhattan"), Address("Brooklyn") } };

            var billing = Serialize(denied)["billing_address"]!;

            Assert.Equal(JTokenType.Object, billing.Type);
            Assert.Equal("Manhattan", billing["city"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithLegacyWireFormat_SendsTheFirstAddressAsAnObject()
        {
            var checkout = new OrderCheckout("1") { BillingAddresses = new[] { Address("Manhattan"), Address("Brooklyn") } };

            var billing = Serialize(checkout, legacy: true)["billing_address"]!;

            Assert.Equal(JTokenType.Object, billing.Type);
            Assert.Equal("Manhattan", billing["city"]!.Value<string>());
        }

        // Orders can't be created by DeserializeObject (their constructor validates merchantOrderId, which
        // the JSON calls "id"; unchanged from 5.7.x), so this populates an existing order, which goes
        // through the same private wire setters.
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_EitherShape_RoundTrips(bool legacy)
        {
            var original = new OrderCheckout("1") { BillingAddress = Address("Manhattan"), ShippingAddress = Address("Queens") };
            var json = JsonConvert.SerializeObject(original, HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null));

            var copy = new OrderCheckout("1");
            JsonConvert.PopulateObject(json, copy);

            Assert.Equal("Manhattan", copy.BillingAddress.City);
            Assert.Equal("Queens", copy.ShippingAddress.City);
        }
    }
}
