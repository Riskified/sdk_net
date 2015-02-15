using Newtonsoft.Json;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderElements
{
    public class FulfillmentDetails : IJsonSerializable
    {

        public FulfillmentDetails(string fulfillmentId, DateTime? createdAt, string status, LineItem[] lineItems = null, string trackingCompany = null, string trackingNumbers = null,
            string trackingUrls = null, string message = null, string receipt = null)
        {
            this.FulfillmentId = fulfillmentId;
            this.CreatedAt = createdAt;
            this.Status = status;

            // Optional fields
            this.LineItems = lineItems;
            this.TrackingCompany = trackingCompany;
            this.TrackingNumbers = trackingNumbers;
            this.TrackingUrls = trackingUrls;
            this.Message = message;
            this.Receipt = receipt;
        }
        

        public void Validate(bool isWeak = false)
        {
            InputValidators.ValidateValuedString(FulfillmentId, "Fulfillment Id");
            InputValidators.ValidateObjectNotNull(CreatedAt, "Created At");
            InputValidators.ValidateDateNotDefault((DateTime)CreatedAt, "Created At");
            InputValidators.ValidateValuedString(Status, "Status");
            if (!EnumUtil.GetDescriptions(typeof(StatusCode)).Contains(Status))
            {
                throw new Exceptions.OrderFieldBadFormatException("Status code is not valid.");
            }

            if(LineItems != null)
            {
                LineItems.ToList().ForEach(item => item.Validate(isWeak));
            }

            
        }

        /// <summary>
        /// Unique identifier of this fulfillment attempt.
        /// </summary>
        [JsonProperty(PropertyName = "fulfillment_id")]
        public string FulfillmentId { get; set; }

        /// <summary>
        /// When the order was fulfilled.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The fulfillment status, Valid values are: success, cancelled, error, failure.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// A list of each line item in the attempted fulfillment.
        /// </summary>
        [JsonProperty(PropertyName = "line_items")]
        public LineItem[] LineItems { get; set; }

        /// <summary>
        /// The name of the shipping company.
        /// </summary>
        [JsonProperty(PropertyName = "tracking_company")]
        public string TrackingCompany { get; set; }

        /// <summary>
        /// A list of shipping numbers, provided by the shipping company.
        /// </summary>
        [JsonProperty(PropertyName = "tracking_numbers")]
        public string TrackingNumbers { get; set; }

        /// <summary>
        /// The URLs to track the fulfillment.
        /// </summary>
        [JsonProperty(PropertyName = "tracking_urls")]
        public string TrackingUrls { get; set; }

        /// <summary>
        /// Additional textual description regarding the fulfillment status.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Additional textual description regarding the fulfillment status.
        /// </summary>
        [JsonProperty(PropertyName = "receipt")]
        public string Receipt { get; set; }



    }
}
