using Newtonsoft.Json;
using Riskified.SDK.Model;
using Riskified.SDK.Model.Internal;
using Xunit;

namespace Riskified.SDK.Tests.Model
{
    /// <summary>
    /// Parses one notification body the way OrdersGateway does (JsonConvert with default settings),
    /// through both wrappers: "order" (/decide, /submit, /decision...) and "checkout"
    /// (/checkout_create, /advise, /checkout_denied). Every parsed field must reach OrderNotification.
    /// </summary>
    public class OrderNotificationTests
    {
        private const string NotificationJson = """
            {
              "id": "golden-1001",
              "status": "declined",
              "oldStatus": "submitted",
              "description": "Reviewed and declined",
              "custom": { "app_dom_id": "D2C" },
              "category": "fraud",
              "action": "decline",
              "decision_code": "R12",
              "score": 87,
              "authentication_type": { "auth_type": "sca", "exemption_method": "tra", "description": "3DS required" },
              "advice": { "in_regulatory_scope": true, "safe_order": false },
              "policy_protect": { "use_cases": [ { "use_case": "return_abuse", "decision": "approve" } ] },
              "recovery_eligibility": { "id": "rec-1", "status": "eligible", "verified_at": "2026-03-01T10:00:00Z", "challenge_type": "otp" },
              "risk_score": 0.87,
              "risk_indicators": { "proxy": true }
            }
            """;

        public static TheoryData<string> WrapperKeys => new TheoryData<string>("order", "checkout");

        [Theory]
        [MemberData(nameof(WrapperKeys))]
        public void Constructor_CopiesEveryParsedField(string wrapperKey)
        {
            var notification = Parse(wrapperKey);

            Assert.Equal("golden-1001", notification.Id);
            Assert.Equal("declined", notification.Status);
            Assert.Equal("submitted", notification.OldStatus);
            Assert.Equal("Reviewed and declined", notification.Description);
            Assert.Equal("D2C", notification.Custom.AppDomId);
            Assert.Equal("fraud", notification.Category);
            Assert.Equal("decline", notification.Action);
            Assert.Equal("R12", notification.DecisionCode);
            Assert.Equal(new[] { "w1" }, notification.Warnings);
            Assert.Equal(87, notification.Score);
            Assert.Equal("sca", notification.AuthenticationType.AuthType);
            Assert.Equal("tra", notification.AuthenticationType.ExemptionMethod);
            Assert.True(notification.Advice.RegulatoryScope);
            Assert.Equal("return_abuse", Assert.Single(notification.PolicyProtect.Policies).PolicyType);
            Assert.Equal("eligible", notification.RecoveryEligibility.Status);
            Assert.Equal(0.87, notification.RiskScore);
            Assert.Equal(true, notification.RiskIndicators["proxy"]);
        }

        private static OrderNotification Parse(string wrapperKey)
        {
            var body = $$"""{ "{{wrapperKey}}": {{NotificationJson}}, "warnings": ["w1"] }""";

            return wrapperKey == "order"
                ? new OrderNotification(JsonConvert.DeserializeObject<OrderWrapper<Notification>>(body)!)
                : new OrderNotification(JsonConvert.DeserializeObject<OrderCheckoutWrapper<Notification>>(body)!);
        }
    }
}
