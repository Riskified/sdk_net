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
    /// Step 3.2: charge_free_payment_details is sent as an array on every order type, bridged from the
    /// existing single ChargeFreePaymentDetails property. The list rules are covered by OrderBaseAddressesTests.
    /// </summary>
    public class OrderBaseChargeFreeTests
    {
        private static JObject Serialize(object obj, bool legacy = false)
        {
            var settings = HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null);
            return JObject.Parse(JsonConvert.SerializeObject(obj, settings));
        }

        public static TheoryData<OrderBase> OrderTypes => new TheoryData<OrderBase>
        {
            new OrderCheckout("1"),
            new OrderCheckoutDenied("1")
        };

        [Theory]
        [MemberData(nameof(OrderTypes))]
        public void Serialize_EveryOrderType_SendsAnArray(OrderBase order)
        {
            order.ChargeFreePaymentDetailsList = new[] { new ChargeFreePaymentDetails("giftcard", 45), new ChargeFreePaymentDetails("store_credit", 10) };

            var chargeFree = (JArray)Serialize(order)["charge_free_payment_details"]!;

            Assert.Equal(new[] { "giftcard", "store_credit" }, chargeFree.Select(c => c["gateway"]!.Value<string>()));
        }

        [Fact]
        public void Serialize_SingleProperty_SendsAnArrayOfOne()
        {
            var order = new OrderCheckout("1") { ChargeFreePaymentDetails = new ChargeFreePaymentDetails("giftcard", 45) };

            var chargeFree = (JArray)Serialize(order)["charge_free_payment_details"]!;

            Assert.Equal(45, Assert.Single(chargeFree)["amount"]!.Value<double>());
        }

        [Fact]
        public void Serialize_WithLegacyWireFormat_SendsTheFirstAsAnObject()
        {
            var order = new OrderCheckout("1")
            {
                ChargeFreePaymentDetailsList = new[] { new ChargeFreePaymentDetails("giftcard", 45), new ChargeFreePaymentDetails("store_credit", 10) }
            };

            var chargeFree = Serialize(order, legacy: true)["charge_free_payment_details"]!;

            Assert.Equal(JTokenType.Object, chargeFree.Type);
            Assert.Equal("giftcard", chargeFree["gateway"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutChargeFreePayments_SendsNothing()
        {
            Assert.Null(Serialize(new OrderCheckout("1"))["charge_free_payment_details"]);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Deserialize_EitherShape_RoundTrips(bool legacy)
        {
            var original = new OrderCheckout("1") { ChargeFreePaymentDetails = new ChargeFreePaymentDetails("giftcard", 45) };
            var json = JsonConvert.SerializeObject(original, HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null));

            var copy = new OrderCheckout("1");
            JsonConvert.PopulateObject(json, copy);

            Assert.Equal("giftcard", copy.ChargeFreePaymentDetails.Gateway);
            Assert.Equal(45, copy.ChargeFreePaymentDetails.Amount);
        }
    }
}
