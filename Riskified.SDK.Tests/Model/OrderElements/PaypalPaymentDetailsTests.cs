using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    /// <summary>
    /// AuthorizationResult (the original, AuthorizationError-typed property) and AuthenticationResult
    /// (the documented shape) both go out as authentication_result.
    /// </summary>
    public class PaypalPaymentDetailsTests
    {
        private static readonly AuthorizationError Error =
            new AuthorizationError(new DateTimeOffset(2026, 3, 3, 8, 0, 0, TimeSpan.Zero), "card_declined", "Card was declined.");

        private static readonly AuthenticationResult Authentication = new AuthenticationResult("05")
        {
            tranStatus = AuthenticationResult.TranStatus.Y,
            cavv = "AAABAWFlmQAAAABjRWWZEEFgFz+="
        };

        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, HttpUtils.CreateSerializerSettings()));
        }

        [Fact]
        public void Serialize_WithAuthorizationResult_SendsItAsBefore()
        {
            var details = new PaypalPaymentDetails("Authorized") { AuthorizationResult = Error };

            var wire = Serialize(details)["authentication_result"]!;

            Assert.Equal("card_declined", wire["error_code"]!.Value<string>());
            Assert.Equal("Card was declined.", wire["message"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithAuthenticationResult_SendsTheDocumentedShape()
        {
            var details = new PaypalPaymentDetails("Authorized") { AuthenticationResult = Authentication };

            var wire = Serialize(details)["authentication_result"]!;

            Assert.Equal("05", wire["eci"]!.Value<string>());
            Assert.Equal("Y", wire["trans_status"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithBoth_SendsAuthenticationResult()
        {
            var details = new PaypalPaymentDetails("Authorized") { AuthorizationResult = Error, AuthenticationResult = Authentication };

            var wire = Serialize(details)["authentication_result"]!;

            Assert.Equal("05", wire["eci"]!.Value<string>());
            Assert.Null(wire["error_code"]);
        }

        [Fact]
        public void Serialize_WithNeither_OmitsTheKey()
        {
            Assert.Null(Serialize(new PaypalPaymentDetails("Authorized"))["authentication_result"]);
        }

        [Fact]
        public void Serialize_DoesNotSendThePublicPropertiesUnderTheirOwnNames()
        {
            var json = Serialize(new PaypalPaymentDetails("Authorized") { AuthorizationResult = Error, AuthenticationResult = Authentication });

            Assert.Null(json["AuthorizationResult"]);
            Assert.Null(json["AuthenticationResult"]);
        }

        [Fact]
        public void Deserialize_AuthorizationResult_RoundTrips()
        {
            var json = JsonConvert.SerializeObject(new PaypalPaymentDetails("Authorized") { AuthorizationResult = Error });

            var details = JsonConvert.DeserializeObject<PaypalPaymentDetails>(json)!;

            Assert.Equal("card_declined", details.AuthorizationResult.ErrorCode);
            Assert.Null(details.AuthenticationResult);
        }

        [Fact]
        public void Deserialize_AuthenticationResult_RoundTrips()
        {
            var json = JsonConvert.SerializeObject(new PaypalPaymentDetails("Authorized") { AuthenticationResult = Authentication });

            var details = JsonConvert.DeserializeObject<PaypalPaymentDetails>(json)!;

            Assert.Equal("05", details.AuthenticationResult.eci);
            Assert.Equal(AuthenticationResult.TranStatus.Y, details.AuthenticationResult.tranStatus);
            Assert.Null(details.AuthorizationResult);
        }
    }
}
