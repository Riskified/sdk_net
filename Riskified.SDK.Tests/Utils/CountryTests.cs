using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Utils
{
    public class CountryTests
    {
        [Theory]
        [InlineData("US")]
        [InlineData("GB")]
        [InlineData("FR")]
        [InlineData("DE")]
        [InlineData("JP")]
        [InlineData("BR")]
        [InlineData("AU")]
        [InlineData("CA")]
        [InlineData("CN")]
        [InlineData("ZW")]
        public void IsValid_ValidUppercaseCode_ReturnsTrue(string code)
        {
            Assert.True(Country.IsValid(code));
        }

        [Theory]
        [InlineData("us")]
        [InlineData("gb")]
        [InlineData("fr")]
        [InlineData("Us")]
        [InlineData("uS")]
        public void IsValid_ValidLowercaseOrMixedCode_ReturnsTrue(string code)
        {
            Assert.True(Country.IsValid(code));
        }

        [Theory]
        [InlineData("XX")]
        [InlineData("ZZ")]
        [InlineData("EU")]
        public void IsValid_TwoLetterCodeNotInIso3166_ReturnsFalse(string code)
        {
            Assert.False(Country.IsValid(code));
        }

        [Theory]
        [InlineData("USA")]
        [InlineData("GBR")]
        [InlineData("FRA")]
        public void IsValid_ThreeLetterCode_ReturnsFalse(string code)
        {
            Assert.False(Country.IsValid(code));
        }

        [Theory]
        [InlineData("A")]
        [InlineData("U")]
        public void IsValid_SingleLetterCode_ReturnsFalse(string code)
        {
            Assert.False(Country.IsValid(code));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("1A")]
        [InlineData("U1")]
        public void IsValid_InvalidFormat_ReturnsFalse(string code)
        {
            Assert.False(Country.IsValid(code));
        }

        [Theory]
        [InlineData("11")]
        [InlineData("26")]
        [InlineData("94")]
        public void IsValid_NumericString_ReturnsFalse(string code)
        {
            Assert.False(Country.IsValid(code));
        }

        [Fact]
        public void IsValid_NullCode_ReturnsFalse()
        {
            Assert.False(Country.IsValid(null));
        }
    }
}
