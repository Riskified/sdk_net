using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    public class OrderDecision : AbstractOrder
    {
        public OrderDecision(int merchantOrderId, DecisionDetails decision, IPaymentDetails[] paymentDetails = null)
            : base(merchantOrderId)
        {
            this.Decision = decision;
        }

        public OrderDecision(string merchantOrderId, DecisionDetails decision, IPaymentDetails[] paymentDetails = null)
            : base(merchantOrderId)
        {
            this.Decision = decision;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level of the model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
            InputValidators.ValidateObjectNotNull(this.Decision, "Decision");
            this.Decision.Validate(validationType);
        }

        /// <summary>
        /// An object containing information about the decision the merchant made on the order.
        /// </summary>
        [JsonProperty(PropertyName = "decision")]
        public DecisionDetails Decision { get; set; }

        [JsonProperty(PropertyName = "payment_details")]
        public IPaymentDetails[] PaymentDetails { get; set; }

        [JsonProperty(PropertyName = "submission_reason")]
        public string SubmissionReason { get; set; }
    }
}
