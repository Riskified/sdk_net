using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Model.OrderElements;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    /// <summary>
    /// New enum members are appended, so members that existed in 5.6.x keep their numeric values.
    /// </summary>
    public class EnumValuesTests
    {
        [Fact]
        public void ExistingMembers_KeepTheirNumericValues()
        {
            Assert.Equal(7, (int)AuthenticationResult.TranStatus.I);
            Assert.Equal(8, (int)ProductType.Remittance);
            Assert.Equal(3, (int)TransportMethodType.Train);
            Assert.Equal(15, (int)PaymentType.RewardPoints);
            Assert.Equal(22, (int)PaymentType.Other);
        }

        [Fact]
        public void RewardPoints_KeepsItsEnumMemberValue()
        {
            Assert.Equal("\"reward_points\"", JsonConvert.SerializeObject(PaymentType.RewardPoints, new StringEnumConverter()));
        }

        [Fact]
        public void NewMembers_SerializeToTheDocumentedValues()
        {
            var converter = new StringEnumConverter();

            Assert.Equal("\"S\"", JsonConvert.SerializeObject(AuthenticationResult.TranStatus.S, converter));
            Assert.Equal("\"crypto\"", JsonConvert.SerializeObject(ProductType.Crypto, converter));
            Assert.Equal("\"giftcard\"", JsonConvert.SerializeObject(ProductType.GiftCard, converter));
            Assert.Equal("\"tram\"", JsonConvert.SerializeObject(TransportMethodType.Tram, converter));
            Assert.Equal("\"rewards_points\"", JsonConvert.SerializeObject(PaymentType.RewardsPoints, converter));
        }
    }
}
