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
    public class OrderCheckoutDenied : AbstractOrder
    {
        /// <summary>
        /// Create a checkout denied
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="authorizationError">An object describing the failed result of an authorization attempt by a payment gateway</param>
        public OrderCheckoutDenied(int merchantOrderId, AuthorizationError authorizationError)
            : base(merchantOrderId)
        {
            this.AuthorizationError = authorizationError;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);

            InputValidators.ValidateObjectNotNull(AuthorizationError, "Authorization Error");
            AuthorizationError.Validate(validationType);


        }

        [JsonProperty(PropertyName = "authorization_error")]
        public AuthorizationError AuthorizationError { get; set; }
    }
}
