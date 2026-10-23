using System.Runtime.Serialization;

namespace Riskified.SDK.Utils
{
    /// <summary>
    /// How the request JSON is shaped. HttpUtils passes it to serialization through
    /// <see cref="Newtonsoft.Json.JsonSerializerSettings.Context"/>.
    /// </summary>
    /// <remarks>
    /// A property getter can't see the serialization context; only serialization callbacks get it.
    /// A model whose JSON depends on these options copies them in an [OnSerializing] callback:
    /// <code>
    /// [JsonIgnore] private bool _legacyWireFormat;
    ///
    /// [OnSerializing]
    /// private void OnSerializing(StreamingContext context)
    /// {
    ///     _legacyWireFormat = WireOptions.IsLegacy(context);
    /// }
    /// </code>
    /// Newtonsoft passes the same context to nested objects, e.g. line items inside an order.
    /// </remarks>
    internal sealed class WireOptions
    {
        public WireOptions(bool legacyWireFormat, bool jsonAcceptHeader = false)
        {
            LegacyWireFormat = legacyWireFormat;
            JsonAcceptHeader = jsonAcceptHeader;
        }

        /// <summary>
        /// Send the 5.7.x JSON shapes and headers instead of the documented ones introduced in 5.8.0.
        /// </summary>
        public bool LegacyWireFormat { get; }

        /// <summary>
        /// Send Accept: application/json instead of the documented vendor value. The OTP endpoint has
        /// only ever been called with application/json.
        /// </summary>
        public bool JsonAcceptHeader { get; }

        /// <summary>
        /// Whether the serialization in progress asked for the 5.7.x shapes. Without options in the
        /// context (e.g. a merchant calling JsonConvert directly), the documented shapes are used.
        /// </summary>
        public static bool IsLegacy(StreamingContext context)
        {
            return context.Context is WireOptions options && options.LegacyWireFormat;
        }
    }
}
