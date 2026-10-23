using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Utils
{
    /// <summary>
    /// The API expects rewards_points (question 4), so the obsolete RewardPoints is sent as rewards_points
    /// too; the compatibility switch keeps the 5.7.x reward_points. Every other payment type is unchanged.
    /// </summary>
    public class PaymentTypeConverterTests
    {
        private static string PaymentTypeSent(object paymentDetails, WireOptions? wireOptions = null)
        {
            var json = JsonConvert.SerializeObject(paymentDetails, HttpUtils.CreateSerializerSettings(wireOptions));
            return JObject.Parse(json)["payment_type"]!.Value<string>()!;
        }

        [Fact]
        public void RewardPoints_IsSentAsRewardsPoints()
        {
            Assert.Equal("rewards_points", PaymentTypeSent(new NonChbPaymentDetails(PaymentType.RewardPoints)));
        }

        [Fact]
        public void RewardPoints_WithLegacyWireFormat_IsSentAsRewardPoints()
        {
            Assert.Equal("reward_points", PaymentTypeSent(new NonChbPaymentDetails(PaymentType.RewardPoints), new WireOptions(legacyWireFormat: true)));
        }

        [Fact]
        public void RewardPoints_WithoutTheSdkSettings_IsSentAsRewardsPoints()
        {
            var json = JsonConvert.SerializeObject(new NonChbPaymentDetails(PaymentType.RewardPoints));

            Assert.Equal("rewards_points", JObject.Parse(json)["payment_type"]!.Value<string>());
        }

        [Fact]
        public void RewardPoints_OnAPaymentWithoutItsOwnConverter_IsSentAsRewardsPoints()
        {
            var wallet = new WalletPaymentDetails(PaymentType.RewardPoints, "auth-1", "Y");

            Assert.Equal("rewards_points", PaymentTypeSent(wallet));
        }

        [Theory]
        [InlineData(PaymentType.RewardsPoints, "rewards_points")]
        [InlineData(PaymentType.GiftCard, "gift_card")]
        [InlineData(PaymentType.StoreCredit, "store_credit")]
        public void OtherPaymentTypes_AreSentAsBefore(PaymentType paymentType, string expected)
        {
            Assert.Equal(expected, PaymentTypeSent(new NonChbPaymentDetails(paymentType)));
            Assert.Equal(expected, PaymentTypeSent(new NonChbPaymentDetails(paymentType), new WireOptions(legacyWireFormat: true)));
        }

        [Theory]
        [InlineData("reward_points", PaymentType.RewardPoints)]
        [InlineData("rewards_points", PaymentType.RewardsPoints)]
        public void Deserialize_ReadsBothValues(string wire, PaymentType expected)
        {
            var details = new NonChbPaymentDetails(PaymentType.GiftCard);

            JsonConvert.PopulateObject("{\"payment_type\": \"" + wire + "\"}", details);

            Assert.Equal(expected, details.PaymentType);
        }
    }
}
