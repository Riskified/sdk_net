using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class WalletPaymentDetailsTests
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

        [Fact]
        public void Constructor_SetsPaymentTypeReturnedByGetter()
        {
            var details = new WalletPaymentDetails(PaymentType.ApplePay, "auth-123", "Y");

            Assert.Equal(PaymentType.ApplePay, details.PaymentType);
            Assert.Equal("auth-123", details.AuthorizationId);
            Assert.Equal("Y", details.AvsResultCode);
        }

        [Theory]
        [InlineData(PaymentType.ApplePay, "apple_pay")]
        [InlineData(PaymentType.GooglePay, "google_pay")]
        [InlineData(PaymentType.SamsungPay, "samsung_pay")]
        [InlineData(PaymentType.WechatPay, "wechat_pay")]
        [InlineData(PaymentType.AmazonPay, "amazon_pay")]
        [InlineData(PaymentType.Alipay, "alipay")]
        public void Serialize_PaymentType_UsesSnakeCaseWalletValue(PaymentType paymentType, string expected)
        {
            var details = new WalletPaymentDetails(paymentType, "auth-123", "Y");

            var json = Serialize(details);

            Assert.Equal(expected, (string)json["payment_type"]);
        }

        [Fact]
        public void Serialize_NewAndTokenFields_UseCorrectKeys()
        {
            var details = new WalletPaymentDetails(PaymentType.GooglePay, "auth-123", "Y")
            {
                CreditCardToken = "tok_abc",
                InitialPaymentAmount = 19.99f,
                PaymentFrequency = 3,
                BillingAddressId = "billing-1",
                AuthorizationType = AuthorizationType.Verification
            };

            var json = Serialize(details);

            Assert.Equal("tok_abc", (string)json["credit_card_token"]);
            Assert.Equal(19.99f, (float)json["initial_payment_amount"]);
            Assert.Equal(3, (int)json["payment_frequency"]);
            Assert.Equal("billing-1", (string)json["billing_address_id"]);
            Assert.Equal("verification", (string)json["authorization_type"]);
        }

        [Fact]
        public void Serialize_UnsetOptionals_AreOmitted()
        {
            var details = new WalletPaymentDetails(PaymentType.ApplePay, "auth-123", "Y");

            var json = Serialize(details);

            Assert.Null(json["installments"]);
            Assert.Null(json["initial_payment_amount"]);
            Assert.Null(json["payment_frequency"]);
            Assert.Null(json["billing_address_id"]);
            Assert.Null(json["authorization_type"]);
        }

        [Fact]
        public void Validate_HappyPath_DoesNotThrow()
        {
            var details = new WalletPaymentDetails(PaymentType.ApplePay, "auth-123", "Y")
            {
                CreditCardCountry = "US",
                AcquirerRegion = "EU",
                ExpiryMonth = 12,
                ExpiryYear = 2030
            };

            details.Validate(Validations.All);
        }

        [Fact]
        public void Validate_NonWalletPaymentType_Throws()
        {
            var details = new WalletPaymentDetails(PaymentType.Card, "auth-123", "Y");

            Assert.Throws<OrderFieldBadFormatException>(() => details.Validate(Validations.All));
        }

        [Fact]
        public void Validate_MissingRequiredAuthorizationId_Throws()
        {
            var details = new WalletPaymentDetails(PaymentType.ApplePay, null, "Y");

            Assert.Throws<OrderFieldBadFormatException>(() => details.Validate(Validations.All));
        }

        [Fact]
        public void Validate_MissingRequiredAvsResultCode_Throws()
        {
            var details = new WalletPaymentDetails(PaymentType.ApplePay, "auth-123", null);

            Assert.Throws<OrderFieldBadFormatException>(() => details.Validate(Validations.All));
        }

        [Fact]
        public void Serialize_InsidePaymentDetailsList_EmitsPaymentTypeDiscriminator()
        {
            IPaymentDetails[] paymentDetails =
            {
                new WalletPaymentDetails(PaymentType.SamsungPay, "auth-123", "Y")
            };

            var json = JArray.Parse(JsonConvert.SerializeObject(paymentDetails, SdkSettings));

            Assert.Single(json);
            Assert.Equal("samsung_pay", (string)json[0]["payment_type"]);
        }
    }
}
