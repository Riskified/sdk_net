using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    /// <summary>
    /// Step 3.5: keys whose documented spelling differs from what the SDK sent go out under both names,
    /// except with the compatibility switch on, which sends only the 5.7.x name.
    /// </summary>
    public class RenamedKeysTests
    {
        private static readonly DateTimeOffset UpdatedAt = new DateTimeOffset(2025, 11, 1, 10, 0, 0, TimeSpan.FromHours(-5));

        // Dates stay strings: JObject.Parse would turn them into DateTime and drop the offset that was sent.
        private static JObject Serialize(object obj, bool legacy = false)
        {
            var settings = HttpUtils.CreateSerializerSettings(legacy ? new WireOptions(legacyWireFormat: true) : null);
            var json = JsonConvert.SerializeObject(obj, settings);
            return JsonConvert.DeserializeObject<JObject>(json, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None })!;
        }

        private static RideTicketLineItem Ride()
        {
            return new RideTicketLineItem("Ride", 74, 1, dropoffLatitude: 40.75f, authorizedPayments: 74);
        }

        [Fact]
        public void Ride_SendsBothSpellings()
        {
            var json = Serialize(Ride());

            Assert.Equal(40.75, json["dropoff_latitude"]!.Value<double>());
            Assert.Equal(40.75, json["dropoff_latitiude"]!.Value<double>());
            Assert.Equal(74, json["authorized_payment"]!.Value<double>());
            Assert.Equal(74, json["authorized_payments"]!.Value<double>());
        }

        [Fact]
        public void Kyc_SendsBothSpellings()
        {
            var json = Serialize(new KycDetails { UpdatedAt = UpdatedAt });

            Assert.Equal("2025-11-01T10:00:00-05:00", json["update_at"]!.Value<string>());
            Assert.Equal("2025-11-01T10:00:00-05:00", json["updated_at"]!.Value<string>());
        }

        [Fact]
        public void WithLegacyWireFormat_SendsOnlyThe5_7Names()
        {
            var ride = Serialize(Ride(), legacy: true);
            var kyc = Serialize(new KycDetails { UpdatedAt = UpdatedAt }, legacy: true);

            Assert.Null(ride["dropoff_latitude"]);
            Assert.Null(ride["authorized_payment"]);
            Assert.NotNull(ride["dropoff_latitiude"]);
            Assert.NotNull(ride["authorized_payments"]);
            Assert.Null(kyc["update_at"]);
            Assert.NotNull(kyc["updated_at"]);
        }

        [Fact]
        public void Unset_SendsNeitherSpelling()
        {
            var ride = Serialize(new RideTicketLineItem("Ride", 74, 1));
            var kyc = Serialize(new KycDetails());

            Assert.All(new[] { "dropoff_latitude", "dropoff_latitiude", "authorized_payment", "authorized_payments" }, key => Assert.Null(ride[key]));
            Assert.Null(kyc["update_at"]);
            Assert.Null(kyc["updated_at"]);
        }

        [Fact]
        public void AuthorizedPayment_SharesTheValueWithAuthorizedPayments()
        {
            var ride = new RideTicketLineItem("Ride", 74, 1) { AuthorizedPayment = 50 };

            Assert.Equal(50, ride.AuthorizedPayments);

            ride.AuthorizedPayments = 60;

            Assert.Equal(60, ride.AuthorizedPayment);
        }

        [Theory]
        [InlineData("{\"dropoff_latitude\": 40.75, \"authorized_payment\": 74}")]
        [InlineData("{\"dropoff_latitiude\": 40.75, \"authorized_payments\": 74}")]
        public void Deserialize_EitherSpelling_SetsTheValue(string json)
        {
            var ride = new RideTicketLineItem("Ride", 74, 1);

            JsonConvert.PopulateObject(json, ride);

            Assert.Equal(40.75f, ride.DropoffLatitude);
            Assert.Equal(74, ride.AuthorizedPayment);
        }

        [Theory]
        [InlineData("{\"update_at\": \"2025-11-01T10:00:00-05:00\"}")]
        [InlineData("{\"updated_at\": \"2025-11-01T10:00:00-05:00\"}")]
        public void Deserialize_KycEitherSpelling_SetsUpdatedAt(string json)
        {
            var kyc = JsonConvert.DeserializeObject<KycDetails>(json)!;

            Assert.Equal(UpdatedAt, kyc.UpdatedAt);
        }
    }
}
