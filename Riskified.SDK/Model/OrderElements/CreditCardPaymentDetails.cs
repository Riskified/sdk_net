using Newtonsoft.Json;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

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
        public CreditCardPaymentDetails(string avsResultCode, 
                                        string cvvResultCode, 
                                        string creditCardBin, 
                                        string creditCardCompany, 
                                        string creditCardNumber, 
                                        string creditCardToken = null)
        {
            AvsResultCode = avsResultCode;
            CvvResultCode = cvvResultCode;
            CreditCardBin = creditCardBin;
            CreditCardCompany = creditCardCompany;
            CreditCardNumber = creditCardNumber;
            CreditCardToken = creditCardToken;
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
    }

}
