using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class CustomerTests
    {
        private static readonly string[] AddedIn5_7 =
        {
            "phone", "tags", "user_name", "group_name", "last_order_id", "referrer_customer_id", "accepts_marketing",
            "kyc_verified", "phone_mandatory", "buy_attempts", "sell_attempts", "gender", "social_signup_type"
        };

        [Fact]
        public void Serialize_WithNewPropertiesUnset_SendsNoneOfThem()
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(new Customer("John", "Doe", "405050606"), HttpUtils.CreateSerializerSettings()));

            Assert.All(AddedIn5_7, key => Assert.Null(json[key]));
        }

        [Fact]
        public void Serialize_WithoutTheSdkSettings_SendsEnumsAsStrings()
        {
            var customer = new Customer("John", "Doe", "405050606") { Gender = Gender.Female, SocialSignupType = SocialType.LinkedIn };

            var json = JObject.Parse(JsonConvert.SerializeObject(customer));

            Assert.Equal("female", json["gender"]!.Value<string>());
            Assert.Equal("linkedin", json["social_signup_type"]!.Value<string>());
        }
    }
}
