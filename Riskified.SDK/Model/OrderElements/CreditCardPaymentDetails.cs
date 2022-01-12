using Newtonsoft.Json;
using System;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Utils;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Riskified.SDK.Model.OrderElements
{
    public class CreditCardPaymentDetails : IPaymentDetails
    {
       

        /// <summary>
        /// The payment information for the order
        /// </summary>
        /// <param name="avsResultCode">The Response code from AVS the address veriﬁcation system. The code is a single letter</param>
        /// <param name="cvvResultCode">The Response code from the credit card company indicating whether the customer entered the card security code, a.k.a. card veriﬁcation value, correctly. The code is a single letter or empty string</param>
        /// <param name="creditCardBin">The issuer identiﬁcation number (IIN), formerly known as bank identiﬁcation number (BIN) ] of the customer's credit card. This is made up of the ﬁrst few digits of the credit card number</param>
        /// <param name="creditCardCompany">The name of the company who issued the customer's credit card</param>
        /// <param name="creditCardNumber">The 4 last digits of the customer's credit card number, with most of the leading digits redacted with Xs</param>
        /// <param name="authorizationId">Unique identifier of the payment transaction as granted by the processing gateway.</param>
        public CreditCardPaymentDetails(string avsResultCode, 
                                        string cvvResultCode, 
                                        string creditCardBin, 
                                        string creditCardCompany, 
                                        string creditCardNumber, 
                                        string authorizationId = null,
                                        string creditCardToken = null, 
                                        DateTimeOffset? storedPaymentCreatedAt = null,
                                        DateTimeOffset? storedPaymentUpdatedAt = null,
                                        int? installments = null)
        {
            AvsResultCode = avsResultCode;
            CvvResultCode = cvvResultCode;
            CreditCardBin = creditCardBin;
            CreditCardCompany = creditCardCompany;
            CreditCardNumber = creditCardNumber;
            AuthorizationId = authorizationId;
            CreditCardToken = creditCardToken;
            StoredPaymentCreatedAt = storedPaymentCreatedAt;
            StoredPaymentUpdatedAt = storedPaymentUpdatedAt;
            Installments = installments;
            PaymentType = PaymentType.CreditCard;
        }



        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            if (validationType != Validations.Weak)
            {
                InputValidators.ValidateAvsResultCode(AvsResultCode);
                InputValidators.ValidateCvvResultCode(CvvResultCode);
                InputValidators.ValidateCreditCard(CreditCardNumber);
            }
            
            InputValidators.ValidateValuedString(CreditCardBin, "Credit Card Bin");
            InputValidators.ValidateValuedString(CreditCardCompany, "Credit Card Company");
        }



        [JsonProperty(PropertyName = "payment_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentType PaymentType { get; set; }

        [JsonProperty(PropertyName = "avs_result_code")]
        public string AvsResultCode { get; set; }

        [JsonProperty(PropertyName = "credit_card_bin")]
        public string CreditCardBin { get; set; }

        [JsonProperty(PropertyName = "credit_card_company")]
        public string CreditCardCompany { get; set; }

        [JsonProperty(PropertyName = "credit_card_number")]
        public string CreditCardNumber { get; set; }

        [JsonProperty(PropertyName = "cvv_result_code")]
        public string CvvResultCode { get; set; }

        [JsonProperty(PropertyName = "credit_card_token")]
        public string CreditCardToken { get; set; }

        [JsonProperty(PropertyName = "authorization_error")]
        public AuthorizationError AuthorizationError { get; set; }
        
        [JsonProperty(PropertyName = "cardholder_name")]
        public string CardholderName { get; set; }

        [JsonProperty(PropertyName = "authorization_id")]
        public string AuthorizationId { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        [JsonProperty(PropertyName = "acquirer_bin")]
        public string AcquirerBin { get; set; }

        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        [JsonProperty(PropertyName = "authentication_result")]
        public AuthenticationResult AuthenticationResult { get; set; }

        [JsonProperty(PropertyName = "stored_payment_created_at")]
        public DateTimeOffset? StoredPaymentCreatedAt { get; set; }

        [JsonProperty(PropertyName = "stored_payment_updated_at")]
        public DateTimeOffset? StoredPaymentUpdatedAt { get; set; }

        [JsonProperty(PropertyName = "installments")]
        public int? Installments { get; set; }

    }

}
