using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public abstract class AbstractOrder : IJsonSerializable
    {
        /// <summary>
        /// @Deprecated - old - replaced by string ctor with order id other than int
        /// </summary>
        protected AbstractOrder(int merchantOrderId)
        {
            Id = merchantOrderId.ToString();
        }

        protected AbstractOrder(string merchantOrderId)
        {
            InputValidators.ValidateValuedString(merchantOrderId, "Merchant Order ID");
            Id = merchantOrderId;
        }

        public virtual void Validate(bool isWeak = false)
        {
            InputValidators.ValidateValuedString(Id, "Merchant Order ID");
        }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }
    }
}
