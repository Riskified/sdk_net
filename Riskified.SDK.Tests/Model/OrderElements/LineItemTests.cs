using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class LineItemTests
    {
        private static readonly string[] AddedIn5_7 =
            { "size", "drop_point", "shipping_address_id", "vendor", "cancellation_policy", "damage_excess" };

        public static TheoryData<LineItem> LineItems => new TheoryData<LineItem>(
            new LineItem("Bag", 10, 1),
            new EventTicketLineItem("Concert", 10, 1),
            new TravelTicketLineItem("Flight", 10, 1),
            new RideTicketLineItem("Ride", 10, 1));

        [Theory]
        [MemberData(nameof(LineItems))]
        public void Serialize_WithNewPropertiesUnset_SendsNoneOfThem(LineItem lineItem)
        {
            var json = JObject.Parse(JsonConvert.SerializeObject(lineItem, HttpUtils.CreateSerializerSettings()));

            Assert.All(AddedIn5_7, key => Assert.Null(json[key]));
        }

        [Fact]
        public void Serialize_Vendor_SendsBothFields()
        {
            var lineItem = new LineItem("Bag", 10, 1) { Vendor = new Vendor { VendorId = "vendor-7", VendorName = "Acme Outlet" } };

            var vendor = JObject.Parse(JsonConvert.SerializeObject(lineItem, HttpUtils.CreateSerializerSettings()))["vendor"]!;

            Assert.Equal("vendor-7", vendor["vendor_id"]!.Value<string>());
            Assert.Equal("Acme Outlet", vendor["vendor_name"]!.Value<string>());
        }
    }
}
