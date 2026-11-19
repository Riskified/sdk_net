using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model
{
    public class OrderDecisionTests
    {
        private static readonly DecisionDetails Decision =
            new DecisionDetails(ExternalStatusType.Approved, new DateTimeOffset(2026, 3, 4, 12, 0, 0, TimeSpan.Zero));

        private static readonly IPaymentDetails[] PaymentDetails =
        {
            new CreditCardPaymentDetails("Y", "M", "424242", "Visa", "4242")
        };

        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, HttpUtils.CreateSerializerSettings()));
        }

        [Fact]
        public void IntIdConstructor_AssignsPaymentDetails()
        {
            var orderDecision = new OrderDecision(4001, Decision, PaymentDetails);

            Assert.Same(PaymentDetails, orderDecision.PaymentDetails);
        }

        [Fact]
        public void StringIdConstructor_AssignsPaymentDetails()
        {
            var orderDecision = new OrderDecision("4001", Decision, PaymentDetails);

            Assert.Same(PaymentDetails, orderDecision.PaymentDetails);
        }

        [Fact]
        public void Serialize_WithPaymentDetails_SendsThem()
        {
            var json = Serialize(new OrderDecision("4001", Decision, PaymentDetails));

            Assert.Equal("4242", json["payment_details"]![0]!["credit_card_number"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutPaymentDetails_OmitsTheKey()
        {
            var json = Serialize(new OrderDecision("4001", Decision));

            Assert.Null(json["payment_details"]);
        }
    }
}
