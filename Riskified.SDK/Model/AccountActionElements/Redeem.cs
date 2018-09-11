using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class Redeem : AbstractAccountAction
    {
        public Redeem(string customerId, RedeemType redeemType, ClientDetails clientDetails, SessionDetails sessionDetails) :
        base(customerId, clientDetails, sessionDetails)
        {
            RedeemType = redeemType;
        }

        [JsonProperty(PropertyName = "redeem_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RedeemType RedeemType { get; set; }
    }
}
