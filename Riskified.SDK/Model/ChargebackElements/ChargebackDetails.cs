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
    public class ChargebackDetails : IJsonSerializable
    {
        public ChargebackDetails(string id = null,
                                 DateTimeOffset? charegbackAt = null, 
                                 string chargebackCurrency = null, 
                                 float? chargebackAmount = null,
                                 string reasonCode = null,
                                 string reasonDesc = null,
                                 string type = null,
                                 string mid = null,
                                 string arn = null,
                                 string creditCardCompany = null,
                                 DateTimeOffset? respondBy = null,
                                 float? feeAmount = null,
                                 string feeCurrency = null,
                                 string cardIssuer = null,
                                 string gateway = null,
                                 string cardholder = null,
                                 string message = null)
        {
            this.Id = id;
            this.ChargebackAt = charegbackAt;
            this.ChargebackCurrency = chargebackCurrency;
            this.ChargebackAmount = chargebackAmount;
            this.ReasonCode = reasonCode;
            this.ReasonDescription = reasonDesc;
            this.Type = type;
            this.MID = mid;
            this.ARN = arn;
            this.CreditCardCompany = creditCardCompany;
            this.RespondBy = respondBy;
            this.FeeAmount = feeAmount;
            this.FeeCurrency = feeCurrency;
            this.CardIssuer = cardIssuer;
            this.Gateway = gateway;
            this.Cardholder = cardholder;
            this.Message = message;
        }

        public void Validate(Utils.Validations validationType = Validations.Weak)
        {
        }

        /// <summary>
        /// The chargeback notice id (if applicable).
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The chargeback date, as recieved from the acquirer.
        /// </summary>
        [JsonProperty(PropertyName = "chargeback_at")]
        public DateTimeOffset? ChargebackAt { get; set; }

        /// <summary>
        /// The chargeback currency, ISO 4217.
        /// </summary>
        [JsonProperty(PropertyName = "chargeback_currency")]
        public string ChargebackCurrency { get; set; }

        /// <summary>
        /// The chargeback amount as stated in the chargeback notice.
        /// </summary>
        [JsonProperty(PropertyName = "chargeback_amount")]
        public float? ChargebackAmount { get; set; }

        /// <summary>
        /// The chargeback reason code, as recieved from the acquirer.
        /// </summary>
        [JsonProperty(PropertyName = "reason_code")]
        public string ReasonCode { get; set; }

        /// <summary>
        /// The chargeback reason description, as recieved from the acquirer.
        /// </summary>
        [JsonProperty(PropertyName = "reason_description")]
        public string ReasonDescription { get; set; }

        /// <summary>
        /// The chargeback transaction type, as received from the acquirer
        /// rfi - Request for Information.
        /// cb - Notification of Chargeback.
        /// cb2 - Second Chargeback.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The merchant account id at the payment gateway
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public string MID { get; set; }

        /// <summary>
        /// Acquirer Reference Number (ARN) A unique number that tags a credit card transaction when it goes from the 
        /// merchant's bank (the acquiring bank) through the card scheme to the cardholder's bank (the issuer).
        /// </summary>
        [JsonProperty(PropertyName = "arn")]
        public string ARN { get; set; }

        /// <summary>
        /// Credit card brand: VISA, Mastercard, AMEX, JCB, etc.
        /// </summary>
        [JsonProperty(PropertyName = "credit_card_company")]
        public string CreditCardCompany { get; set; }

        /// <summary>
        /// Last date to challenge CHB
        /// </summary>
        [JsonProperty(PropertyName = "respond_by")]
        public DateTimeOffset? RespondBy { get; set; }

        /// <summary>
        /// The chargeback fee amount
        /// </summary>
        [JsonProperty(PropertyName = "fee_amount")]
        public float? FeeAmount { get; set; }
        
        /// <summary>
        /// The chargeback fee currency
        /// </summary>
        [JsonProperty(PropertyName = "fee_currency")]
        public string FeeCurrency { get; set; }

        /// <summary>
        /// The card issuer
        /// </summary>
        [JsonProperty(PropertyName = "card_issuer")]
        public string CardIssuer { get; set; }

        /// <summary>
        /// The payment gateway who processed the order
        /// </summary>
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        /// <summary>
        /// The identifier of the person who submitted the CHB, as it appears on the chargeback notice
        /// </summary>
        [JsonProperty(PropertyName = "cardholder")]
        public string Cardholder { get; set; }

        /// <summary>
        /// Optional issuer message
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
