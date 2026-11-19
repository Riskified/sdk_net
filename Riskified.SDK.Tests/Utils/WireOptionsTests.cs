using System.Runtime.Serialization;
using Newtonsoft.Json;
using Riskified.SDK.Orders;
using Riskified.SDK.Utils;
using Xunit;

namespace Riskified.SDK.Tests.Utils
{
    /// <summary>
    /// The compatibility switch (step 3.0): OrdersGateway.UseLegacyWireFormat reaches every model being
    /// serialized through the serialization context, where [OnSerializing] callbacks read it.
    /// </summary>
    public class WireOptionsTests
    {
        // Stands in for a model with a bridge: records what its callback saw, and nests a child.
        private class Probe
        {
            [JsonIgnore]
            public bool SawLegacy { get; private set; }

            [JsonProperty(PropertyName = "child")]
            public Probe Child { get; set; }

            [OnSerializing]
            private void OnSerializing(StreamingContext context)
            {
                SawLegacy = WireOptions.IsLegacy(context);
            }
        }

        [Fact]
        public void Serialize_WithLegacyOptions_EveryNestedObjectSeesThem()
        {
            var probe = new Probe { Child = new Probe { Child = new Probe() } };

            JsonConvert.SerializeObject(probe, HttpUtils.CreateSerializerSettings(new WireOptions(legacyWireFormat: true)));

            Assert.True(probe.SawLegacy);
            Assert.True(probe.Child.SawLegacy);
            Assert.True(probe.Child.Child!.SawLegacy);
        }

        [Fact]
        public void Serialize_WithDocumentedOptions_IsNotLegacy()
        {
            var probe = new Probe { Child = new Probe() };

            JsonConvert.SerializeObject(probe, HttpUtils.CreateSerializerSettings(new WireOptions(legacyWireFormat: false)));

            Assert.False(probe.SawLegacy);
            Assert.False(probe.Child.SawLegacy);
        }

        [Fact]
        public void Serialize_WithoutOptions_UsesTheDocumentedShapes()
        {
            var withSdkDefaults = new Probe();
            var withPlainJsonConvert = new Probe();

            JsonConvert.SerializeObject(withSdkDefaults, HttpUtils.CreateSerializerSettings());
            JsonConvert.SerializeObject(withPlainJsonConvert);

            Assert.False(withSdkDefaults.SawLegacy);
            Assert.False(withPlainJsonConvert.SawLegacy);
        }

        [Fact]
        public void OrdersGateway_DefaultsToTheDocumentedShapes()
        {
            var gateway = new OrdersGateway(RiskifiedEnvironment.Sandbox, "token", "shop.example.com");

            Assert.False(gateway.WireOptions.LegacyWireFormat);
        }

        [Fact]
        public void OrdersGateway_UseLegacyWireFormat_ReachesTheWireOptions()
        {
            var gateway = new OrdersGateway(RiskifiedEnvironment.Sandbox, "token", "shop.example.com");

#pragma warning disable CS0618 // the switch is obsolete by design
            gateway.UseLegacyWireFormat = true;
#pragma warning restore CS0618

            Assert.True(gateway.WireOptions.LegacyWireFormat);
        }
    }
}
