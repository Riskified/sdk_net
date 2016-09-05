using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.ChargebackElements
{
    public class DisputeDetails : IJsonSerializable
    {

        public DisputeDetails(string caseId = null,
                              string status = null,
                              DateTime? disputedAt = null,
                              DateTime? expectedResolutionDate = null,
                              string disputeType = null,
                              string issuerPocPhoneNumber = null)
        {
            this.CaseId = caseId;
            this.Status = status;
            this.DisputedAt = disputedAt;
            this.ExpectedResolutionDate = expectedResolutionDate;
            this.DisputeType = disputeType;
            this.IssuerPocPhoneNumber = issuerPocPhoneNumber;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
        }

        /// <summary>
        /// Dispute identifier as defined by the issuer/gateway
        /// </summary>
        [JsonProperty(PropertyName = "case_id")]
        public string CaseId { get; set; }

        /// <summary>
        /// One of the following:
        /// candidate
        /// ineligible
        /// pending
        /// won
        /// lost
        /// Note: we expect to update the api when the dispute status changes
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// When was the dispute sent
        /// </summary>
        [JsonProperty(PropertyName = "disputed_at")]
        public DateTime? DisputedAt { get; set; }

        /// <summary>
        /// When should we expect a decision from the issuer (60-75 days usually)
        /// </summary>
        [JsonProperty(PropertyName = "expected_resolution_date")]
        public DateTime? ExpectedResolutionDate { get; set; }

        /// <summary>
        /// One of the following:
        /// first_dispute
        /// second_dispute
        /// arbitrary_court
        /// Note: we expect to update the api when the dispute status changes
        /// </summary>
        [JsonProperty(PropertyName = "dispute_type")]
        public string DisputeType { get; set; }

        /// <summary>
        /// Credit card issuer or gateway provider phone number
        /// </summary>
        [JsonProperty(PropertyName = "issuer_poc_phone_number")]
        public string IssuerPocPhoneNumber { get; set; }
    }
}
