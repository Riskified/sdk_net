using Newtonsoft.Json;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    public class OrderCheckoutDenied : OrderBase
    {
        /// <summary>
        /// @Deprecated - Create a checkout denied (deprecated constructor, please use PaymentDetails.AuthorizationError)
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="authorizationError">An object describing the failed result of an authorization attempt by a payment gateway</param>
        public OrderCheckoutDenied(string merchantOrderId, AuthorizationError authorizationError)
            : base(merchantOrderId)
        {
            this.AuthorizationError = authorizationError;
        }

        /// <summary>
        /// Create a checkout denied
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        public OrderCheckoutDenied(string merchantOrderId)
            : base(merchantOrderId)
        {
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        /// <summary>
        /// @Deprecated property, please use PaymentDetails.AuthorizationError
        /// </summary>
        [JsonProperty(PropertyName = "authorization_error")]
        public AuthorizationError AuthorizationError { get; set; }
    }
}
