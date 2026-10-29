using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Golden
{
    /// <summary>
    /// Pins the exact JSON the SDK sends for each golden sample. A diff here means the wire format
    /// changed. To accept an intended change, run the tests with GOLDEN_UPDATE=1 and commit the
    /// regenerated files under Golden/Snapshots.
    /// </summary>
    public class GoldenSnapshotTests
    {
        private static readonly bool UpdateSnapshots = Environment.GetEnvironmentVariable("GOLDEN_UPDATE") == "1";

        public static TheoryData<string> SampleNames => new TheoryData<string>(GoldenOrders.All.Keys);

        [Theory]
        [MemberData(nameof(SampleNames))]
        public void Serialization_MatchesSnapshot(string sampleName)
        {
            var actual = ParseJson(HttpUtils.SerializeRequestBody(GoldenOrders.All[sampleName]));
            var snapshotPath = Path.Combine(GoldenPaths.Snapshots, sampleName + ".json");

            if (UpdateSnapshots)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(snapshotPath)!);
                File.WriteAllText(snapshotPath, actual.ToString(Formatting.Indented) + Environment.NewLine);
                return;
            }

            Assert.True(File.Exists(snapshotPath), $"Missing snapshot {snapshotPath}. Run the tests with GOLDEN_UPDATE=1 to create it.");
            var expected = ParseJson(File.ReadAllText(snapshotPath));

            Assert.True(JToken.DeepEquals(expected, actual),
                $"Serialized '{sampleName}' differs from {snapshotPath}.{Environment.NewLine}" +
                $"Expected:{Environment.NewLine}{expected}{Environment.NewLine}Actual:{Environment.NewLine}{actual}");
        }

        public static TheoryData<string> Snapshot5_7Names => new TheoryData<string>(
            Directory.GetFiles(GoldenPaths.Snapshots5_7, "*.json").Select(path => Path.GetFileNameWithoutExtension(path)));

        // The compatibility switch (OrdersGateway.UseLegacyWireFormat) must send exactly what 5.7.x sent.
        [Theory]
        [MemberData(nameof(Snapshot5_7Names))]
        public void Serialization_WithLegacyWireFormat_MatchesThe5_7Snapshot(string sampleName)
        {
            var actual = ParseJson(HttpUtils.SerializeRequestBody(GoldenOrders.All[sampleName], new WireOptions(legacyWireFormat: true)));
            var snapshotPath = Path.Combine(GoldenPaths.Snapshots5_7, sampleName + ".json");
            var expected = ParseJson(File.ReadAllText(snapshotPath));

            Assert.True(JToken.DeepEquals(expected, actual),
                $"With the compatibility switch on, '{sampleName}' differs from the 5.7.x output in {snapshotPath}.{Environment.NewLine}" +
                $"Expected:{Environment.NewLine}{expected}{Environment.NewLine}Actual:{Environment.NewLine}{actual}");
        }

        [Fact]
        public void EverySnapshotHasASample()
        {
            var orphans = Directory.Exists(GoldenPaths.Snapshots)
                ? Directory.GetFiles(GoldenPaths.Snapshots, "*.json")
                    .Select(Path.GetFileNameWithoutExtension)
                    .Where(name => !GoldenOrders.All.ContainsKey(name!))
                    .ToList()
                : new System.Collections.Generic.List<string?>();

            Assert.Empty(orphans);
        }

        // Dates stay strings: parsing them into DateTime would drop the offset the SDK actually sent.
        private static JToken ParseJson(string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)) { DateParseHandling = DateParseHandling.None })
            {
                return JToken.ReadFrom(reader);
            }
        }
    }
}
