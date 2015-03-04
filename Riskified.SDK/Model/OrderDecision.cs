using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    public class OrderDecision : AbstractOrder
    {
        public OrderDecision(int merchantOrderId, DecisionDetails decision)
            : base(merchantOrderId)
        {
            this.Decision = decision;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
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
    }
}
