using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Model;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Model.OrderElements
{
    public class DecisionDetailsTests
    {
        private static readonly DateTimeOffset DecidedAt = new DateTimeOffset(2026, 3, 4, 12, 0, 0, TimeSpan.Zero);

        private static JObject Serialize(object obj)
        {
            return JObject.Parse(JsonConvert.SerializeObject(obj, HttpUtils.CreateSerializerSettings()));
        }

        [Fact]
        public void Constructor_AssignsNotes()
        {
            var details = new DecisionDetails(ExternalStatusType.Approved, DecidedAt, notes: "reviewed by fraud team");

            Assert.Equal("reviewed by fraud team", details.Notes);
        }

        [Fact]
        public void Serialize_WithNotes_SendsThem()
        {
            var json = Serialize(new DecisionDetails(ExternalStatusType.Approved, DecidedAt, notes: "reviewed by fraud team"));

            Assert.Equal("reviewed by fraud team", json["notes"]!.Value<string>());
        }

        [Fact]
        public void Serialize_WithoutNotes_OmitsTheKey()
        {
            var json = Serialize(new DecisionDetails(ExternalStatusType.Approved, DecidedAt));

            Assert.Null(json["notes"]);
        }
    }
}
