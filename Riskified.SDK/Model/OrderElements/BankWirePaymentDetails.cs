using System;
using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class BankWirePaymentDetails : IPaymentDetails
    {
        /// <summary>
        /// The payment information for the order in case of a bank wire / ach payment
        /// </summary>
        /// <param name="accountNumber">The account number</param>
        /// <param name="routingNumber">The routing number</param>
        public BankWirePaymentDetails(String accountNumber, String routingNumber)
        {
            AccountNumber = accountNumber;
            RoutingNumber = routingNumber;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            if (validationType != Validations.Weak)
            {
                InputValidators.ValidateValuedString(AccountNumber, "Account Number");
                InputValidators.ValidateValuedString(RoutingNumber, "Routing Number");
            }
        }

        [JsonProperty(PropertyName = "account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "routing_number")]
        public string RoutingNumber { get; set; }

        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }

        [JsonProperty(PropertyName = "recipient_name")]
        public string RecipientName { get; set; }
    }
}
