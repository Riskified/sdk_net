using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// A cryptocurrency wallet, of a customer or of a line item's recipient.
    /// </summary>
    public class CryptoWallet : IJsonSerializable
    {
        /// <summary>
        /// Creates a crypto wallet with its required fields.
        /// </summary>
        /// <param name="address">The wallet address.</param>
        /// <param name="network">The blockchain network (e.g. ethereum).</param>
        public CryptoWallet(string address, string network)
        {
            Address = address;
            Network = network;
        }

        public void Validate(Validations validationType = Validations.Weak)
        {
        }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "network")]
        public string Network { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CryptoWalletType? Type { get; set; }
    }
}
