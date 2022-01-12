using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{

    public class DecisionDetails : IJsonSerializable
    {

        public DecisionDetails(ExternalStatusType externalStatus, DateTimeOffset? decidedAt, string reason = null, float? amount = null, string currency = null, string notes = null)
        {
            this.ExternalStatus = externalStatus;
            this.DecidedAt = decidedAt;

            // Optional fields
            this.Reason = reason;
            this.Amount = amount;
            this.Currency = currency;
            this.Notes = Notes;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
            InputValidators.ValidateObjectNotNull(this.ExternalStatus, "External Status");
            if(this.DecidedAt != null)
            {
                InputValidators.ValidateDateNotDefault(DecidedAt.Value, "Decided At");
            }

            if(Currency != null)
            {
                InputValidators.ValidateCurrency(this.Currency);
            }
        }

        /// <summary>
        /// The external status, meaning the merchant decision on the order
        /// </summary>
        [JsonProperty(PropertyName = "external_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExternalStatusType ExternalStatus { get; set; }

        /// <summary>
        /// The date when the order was decided.
        /// </summary>
        [JsonProperty(PropertyName = "decided_at")]
        public DateTimeOffset? DecidedAt { get; set; }

        /// <summary>
        /// A reason for the decision.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        /// <summary>
        /// The amount the decision is relevant on.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public float? Amount { get; set; }

        /// <summary>
        /// The three letter code (ISO 4217) for the currency used for the payment.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Free text for describing the decision.
        /// </summary>
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
