using System;
using System.Reflection;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Orders;
using Xunit;

namespace Riskified.SDK.Tests.Model
{
    /// <summary>
    /// Step 4.1: the bridged members are deprecated with warnings, never errors, so code using them keeps
    /// compiling until 6.0.0. Each message names what to use instead.
    /// </summary>
    public class ObsoleteMembersTests
    {
        public static TheoryData<Type, string, string> Deprecated => new TheoryData<Type, string, string>
        {
            { typeof(OrderBase), nameof(OrderBase.BillingAddress), nameof(OrderBase.BillingAddresses) },
            { typeof(OrderBase), nameof(OrderBase.ShippingAddress), nameof(OrderBase.ShippingAddresses) },
            { typeof(OrderBase), nameof(OrderBase.ChargeFreePaymentDetails), nameof(OrderBase.ChargeFreePaymentDetailsList) },
            { typeof(RideTicketLineItem), nameof(RideTicketLineItem.PickupAddress), nameof(RideTicketLineItem.PickupAddresses) },
            { typeof(RideTicketLineItem), nameof(RideTicketLineItem.DropoffAddress), nameof(RideTicketLineItem.DropoffAddresses) },
            { typeof(RideTicketLineItem), nameof(RideTicketLineItem.AuthorizedPayments), nameof(RideTicketLineItem.AuthorizedPayment) },
            { typeof(OrderCheckoutDenied), nameof(OrderCheckoutDenied.AuthorizationError), "payment details" },
            { typeof(PaypalPaymentDetails), nameof(PaypalPaymentDetails.AuthorizationResult), nameof(PaypalPaymentDetails.AuthenticationResult) },
            { typeof(OrdersGateway), nameof(OrdersGateway.UseLegacyWireFormat), "documented shapes" },
        };

        [Theory]
        [MemberData(nameof(Deprecated))]
        public void Member_IsObsoleteAsAWarningNamingItsReplacement(Type type, string member, string replacement)
        {
            var obsolete = type.GetProperty(member)!.GetCustomAttribute<ObsoleteAttribute>();

            Assert.NotNull(obsolete);
            Assert.False(obsolete!.IsError);
            Assert.Contains(replacement, obsolete.Message);
            Assert.Contains("6.0.0", obsolete.Message);
        }

        [Fact]
        public void RewardPoints_IsObsoleteAsAWarningNamingRewardsPoints()
        {
            var obsolete = typeof(PaymentType).GetField(nameof(PaymentType.RewardPoints))!.GetCustomAttribute<ObsoleteAttribute>();

            Assert.NotNull(obsolete);
            Assert.False(obsolete!.IsError);
            Assert.Contains(nameof(PaymentType.RewardsPoints), obsolete.Message);
            Assert.Contains("6.0.0", obsolete.Message);
        }
    }
}
