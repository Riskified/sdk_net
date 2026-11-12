using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Utils
{
    /// <summary>
    /// Step 3.7: the Accept header is the documented vendor value again (as before 3.9.0), except for the
    /// OTP endpoint and with the compatibility switch on, which keep application/json.
    /// </summary>
    public class AcceptHeaderTests
    {
        private const string Vendor = "application/vnd.riskified.com; version=2";
        private const string Json = "application/json";

        [Fact]
        public void Default_SendsTheDocumentedVendorValue()
        {
            Assert.Equal(Vendor, HttpUtils.AcceptHeader(new WireOptions(legacyWireFormat: false)));
            Assert.Equal(Vendor, HttpUtils.AcceptHeader(null));
        }

        [Fact]
        public void Otp_SendsApplicationJson()
        {
            Assert.Equal(Json, HttpUtils.AcceptHeader(new WireOptions(legacyWireFormat: false, jsonAcceptHeader: true)));
        }

        [Fact]
        public void WithLegacyWireFormat_SendsApplicationJson()
        {
            Assert.Equal(Json, HttpUtils.AcceptHeader(new WireOptions(legacyWireFormat: true)));
            Assert.Equal(Json, HttpUtils.AcceptHeader(new WireOptions(legacyWireFormat: true, jsonAcceptHeader: true)));
        }
    }
}
