using Newtonsoft.Json;

namespace Riskified.SDK.Orders
{
    public class OrderTransactionResult
    {
        [JsonProperty(PropertyName = "order",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public SuccessfulOrderTransactionData SuccessfulResult { get; set; }

        // TODO simulate the case of failure message received from riskified without order...
        [JsonProperty(PropertyName = "error", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public FailedTransactionData FailedResult { get; set; }
        
        /// <summary>
        /// A flag that signs if the transaction was finished successfully
        /// Values of SuccessfulResult and FailedResult will be set accordingly (one will be null)
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful
        {
            get { return SuccessfulResult != null; }
        }
    }

    public class FailedTransactionData
    {
        [JsonProperty(PropertyName = "message",Required = Required.Always)]
        public string ErrorMessage { get; set; }
    }

    public class SuccessfulOrderTransactionData
    {

        [JsonProperty(PropertyName = "id",Required = Required.Always)]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

    }
}
