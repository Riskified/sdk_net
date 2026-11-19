using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Internal;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Utils
{
    /// <summary>
    /// Step 3.4: on /checkout_denied, the order-level authorization_error is copied into each card and
    /// wallet payment details entry that has none of its own, in the request JSON only.
    /// </summary>
    public class CheckoutDeniedAuthorizationErrorTests
    {
        private static readonly DateTimeOffset At = new DateTimeOffset(2026, 3, 3, 8, 0, 0, TimeSpan.Zero);

        private static AuthorizationError RootError => new AuthorizationError(At, "card_declined", "Card was declined.");

        private static CreditCardPaymentDetails Card() => new CreditCardPaymentDetails("Y", "M", "424242", "Visa", "4242");

        private static WalletPaymentDetails Wallet() => new WalletPaymentDetails(PaymentType.ApplePay, "auth-1", "Y");

        private static JArray PaymentDetailsSent(OrderCheckoutDenied order, WireOptions? wireOptions = null)
        {
            var body = HttpUtils.SerializeRequestBody(new OrderCheckoutWrapper<AbstractOrder>(order), wireOptions);
            return (JArray)JObject.Parse(body)["checkout"]!["payment_details"]!;
        }

        [Fact]
        public void CardAndWalletWithoutAnError_GetTheOrderLevelError()
        {
            var order = new OrderCheckoutDenied("1", RootError) { PaymentDetails = new IPaymentDetails[] { Card(), Wallet() } };

            var sent = PaymentDetailsSent(order);

            Assert.All(sent, entry => Assert.Equal("card_declined", entry["authorization_error"]!["error_code"]!.Value<string>()));
        }

        [Fact]
        public void PaymentWithItsOwnError_KeepsIt()
        {
            var card = Card();
            card.AuthorizationError = new AuthorizationError(At, "insufficient_funds");
            var order = new OrderCheckoutDenied("1", RootError) { PaymentDetails = new IPaymentDetails[] { card } };

            var sent = PaymentDetailsSent(order);

            Assert.Equal("insufficient_funds", sent[0]["authorization_error"]!["error_code"]!.Value<string>());
        }

        [Fact]
        public void OtherPaymentTypes_AreLeftAlone()
        {
            var order = new OrderCheckoutDenied("1", RootError)
            {
                PaymentDetails = new IPaymentDetails[] { new PaypalPaymentDetails("Declined"), new BankWirePaymentDetails("1", "2"), Card() }
            };

            var sent = PaymentDetailsSent(order);

            Assert.Null(sent[0]["authorization_error"]);
            Assert.Null(sent[1]["authorization_error"]);
            Assert.NotNull(sent[2]["authorization_error"]);
        }

        [Fact]
        public void CallersPaymentObjects_AreNotChanged()
        {
            var card = Card();
            var order = new OrderCheckoutDenied("1", RootError) { PaymentDetails = new IPaymentDetails[] { card } };

            PaymentDetailsSent(order);

            Assert.Null(card.AuthorizationError);
        }

        [Fact]
        public void OrderLevelError_IsStillSent()
        {
            var order = new OrderCheckoutDenied("1", RootError) { PaymentDetails = new IPaymentDetails[] { Card() } };

            var body = JObject.Parse(HttpUtils.SerializeRequestBody(new OrderCheckoutWrapper<AbstractOrder>(order)));

            Assert.Equal("card_declined", body["checkout"]!["authorization_error"]!["error_code"]!.Value<string>());
        }

        [Fact]
        public void WithLegacyWireFormat_NothingIsCopied()
        {
            var order = new OrderCheckoutDenied("1", RootError) { PaymentDetails = new IPaymentDetails[] { Card(), Wallet() } };

            var sent = PaymentDetailsSent(order, new WireOptions(legacyWireFormat: true));

            Assert.All(sent, entry => Assert.Null(entry["authorization_error"]));
        }

        [Fact]
        public void WithoutAnOrderLevelError_NothingIsCopied()
        {
            var order = new OrderCheckoutDenied("1") { PaymentDetails = new IPaymentDetails[] { Card() } };

            Assert.Null(PaymentDetailsSent(order)[0]["authorization_error"]);
        }

        [Fact]
        public void OtherOrderTypes_AreSerializedAsBefore()
        {
            var checkout = new OrderCheckout("1") { PaymentDetails = new IPaymentDetails[] { Card() } };
            var wrapper = new OrderCheckoutWrapper<AbstractOrder>(checkout);

            Assert.Equal(
                JsonConvert.SerializeObject(wrapper, HttpUtils.CreateSerializerSettings()),
                HttpUtils.SerializeRequestBody(wrapper));
        }
    }
}
