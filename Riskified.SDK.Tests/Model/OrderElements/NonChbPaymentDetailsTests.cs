using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class NonChbPaymentDetailsTests
    {
        [Theory]
        [InlineData(PaymentType.GiftCard, "gift_card")]
        [InlineData(PaymentType.StoreCredit, "store_credit")]
        [InlineData(PaymentType.RewardsPoints, "rewards_points")]
        public void Serialize_SendsTheGivenPaymentType(PaymentType paymentType, string expected)
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new NonChbPaymentDetails(paymentType), HttpUtils.CreateSerializerSettings()));

            Assert.Equal(expected, json["payment_type"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutTheSdkSettings_StillSendsThePaymentTypeAsAString()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new NonChbPaymentDetails(PaymentType.GiftCard)));

            Assert.Equal("gift_card", json["payment_type"]!.Value<string>());
        }
    }
}
