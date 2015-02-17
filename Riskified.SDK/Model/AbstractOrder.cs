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

        public virtual void Validate(Validations validationType = Validations.Weak)
        {
            
            InputValidators.ValidateValuedString(Id, "Merchant Order ID");
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
