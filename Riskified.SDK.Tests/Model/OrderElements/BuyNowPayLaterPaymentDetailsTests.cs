using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class BuyNowPayLaterPaymentDetailsTests
    {
        [Fact]
        public void Serialize_SendsEveryField()
        {
            var details = new BuyNowPayLaterPaymentDetails { InitialPaymentAmount = 250.5, Installments = 4, PaymentFrequency = 14 };

            var json = JObject.Parse(JsonConvert.SerializeObject(details, HttpUtils.CreateSerializerSettings()));

            Assert.Equal("bnpl", json["payment_type"]!.Value<string>());
            Assert.Equal(250.5, json["initial_payment_amount"]!.Value<double>());
            Assert.Equal(4, json["installments"]!.Value<int>());
            Assert.Equal(14, json["payment_frequency"]!.Value<int>());
        }

        [Fact]
        public void Serialize_WithNothingSet_SendsOnlyThePaymentType()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new BuyNowPayLaterPaymentDetails(), HttpUtils.CreateSerializerSettings()));

            Assert.Equal(new[] { "payment_type" }, json.Properties().Select(p => p.Name));
        }

        [Fact]
        public void Serialize_WithoutTheSdkSettings_StillSendsThePaymentTypeAsAString()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new BuyNowPayLaterPaymentDetails()));

            Assert.Equal("bnpl", json["payment_type"]!.Value<string>());
        }
    }
}
