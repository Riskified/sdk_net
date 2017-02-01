using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riskified.SDK.Model.ChargebackElements;

namespace Riskified.SDK.Model
{
    public class OrderChargeback : OrderBase
    {
        /// <summary>
        /// Creates a new order chargeback
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        public OrderChargeback(string merchantOrderId, ChargebackDetails chargebackDetails, FulfillmentDetails fulfillment, DisputeDetails disputeDetails)
            : base(merchantOrderId)
        {
            this.Chargeback = chargebackDetails;
            this.Fulfillment = fulfillment;
            this.Dispute = disputeDetails;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "chargebackDetails")]
        public ChargebackDetails Chargeback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "fulfillmentDetails")]
        public FulfillmentDetails Fulfillment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "disputeDetails")]
        public DisputeDetails Dispute { get; set; }

    }
}
