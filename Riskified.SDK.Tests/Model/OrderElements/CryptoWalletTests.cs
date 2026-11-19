using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class CryptoWalletTests
    {
        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, HttpUtils.CreateSerializerSettings()));
        }

        [Theory]
        [InlineData(CryptoWalletType.Primary, "primary")]
        [InlineData(CryptoWalletType.NonCustodial, "non_custodial")]
        public void Serialize_WithoutTheSdkSettings_SendsTheTypeAsAString(CryptoWalletType type, string expected)
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new CryptoWallet("0xabc", "ethereum") { Type = type }));

            Assert.Equal(expected, json["type"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutType_SendsOnlyAddressAndNetwork()
        {
            var json = Serialize(new CryptoWallet("0xabc", "ethereum"));

            Assert.Equal(new[] { "address", "network" }, json.Properties().Select(p => p.Name));
        }

        [Fact]
        public void Serialize_NewRecipientShippingAndSocialPropertiesUnset_SendsNoneOfThem()
        {
            var recipient = Serialize(new Recipient(email: "a@example.com"));
            var shippingLine = Serialize(new ShippingLine(price: 5, title: "Mail"));
            var social = Serialize(new SocialDetails(network: "facebook", publicUsername: "john"));

            Assert.All(new[] { "crypto_wallet", "bank_code", "created_at", "updated_at", "self_recipient" }, key => Assert.Null(recipient[key]));
            Assert.All(new[] { "company", "shipping_address_id" }, key => Assert.Null(shippingLine[key]));
            Assert.All(new[] { "id", "auth_token", "social_data" }, key => Assert.Null(social[key]));
        }

        [Fact]
        public void Serialize_SocialData_SendsTheDictionaryAsAnObject()
        {
            var social = new SocialDetails(network: "facebook", publicUsername: "john")
            {
                SocialData = new Dictionary<string, object> { ["friends_count"] = 120, ["verified"] = true }
            };

            var socialData = Serialize(social)["social_data"]!;

            Assert.Equal(120, socialData["friends_count"]!.Value<int>());
            Assert.True(socialData["verified"]!.Value<bool>());
        }
    }
}
