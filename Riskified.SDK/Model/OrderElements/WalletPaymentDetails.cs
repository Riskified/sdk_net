using Newtonsoft.Json;
using System;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Model.OrderCheckoutElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// Payment information for digital-wallet payments (Apple Pay, Google Pay, Samsung Pay,
    /// WeChat Pay, Amazon Pay, Alipay). Maps to the API's digital_wallets_payment_details schema.
    /// Unlike the other payment models, <see cref="PaymentType"/> is settable because it varies
    /// across the supported wallet types.
    /// </summary>
    public class WalletPaymentDetails : IPaymentDetails
    {
        /// <summary>
        /// Creates a wallet payment details object with its required fields.
        /// </summary>
        /// <param name="paymentType">The wallet payment type. Must be one of: apple_pay, google_pay, samsung_pay, wechat_pay, amazon_pay, alipay</param>
        /// <param name="authorizationId">Unique identifier of the payment transaction as granted by the processing gateway.</param>
        /// <param name="avsResultCode">The Response code from AVS the address verification system.</param>
        public WalletPaymentDetails(PaymentType paymentType, string authorizationId, string avsResultCode)
        {
            PaymentType = paymentType;
            AuthorizationId = authorizationId;
            AvsResultCode = avsResultCode;
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
                if (!IsWalletPaymentType(PaymentType))
                    throw new OrderFieldBadFormatException(
                        string.Format("Payment Type must be one of the digital-wallet types (apple_pay, google_pay, samsung_pay, wechat_pay, amazon_pay, alipay). Value was \"{0}\"", PaymentType));

                InputValidators.ValidateValuedString(AuthorizationId, "Authorization Id");
                InputValidators.ValidateValuedString(AvsResultCode, "AVS Result Code");

                if (!string.IsNullOrEmpty(CreditCardCountry))
                    InputValidators.ValidateCountryCode(CreditCardCountry);

                if (!string.IsNullOrEmpty(AcquirerRegion) && !AcquirerRegion.Equals("EU") && !AcquirerRegion.Equals("NONEU"))
                    throw new OrderFieldBadFormatException(
                        string.Format("Acquirer Region must be either \"EU\" or \"NONEU\". Value was \"{0}\"", AcquirerRegion));

                if (ExpiryMonth.HasValue && (ExpiryMonth.Value < 1 || ExpiryMonth.Value > 12))
                    throw new OrderFieldBadFormatException(
                        string.Format("Expiry Month must be between 1 and 12. Value was \"{0}\"", ExpiryMonth.Value));

                if (ExpiryYear.HasValue && (ExpiryYear.Value < 1000 || ExpiryYear.Value > 9999))
                    throw new OrderFieldBadFormatException(
                        string.Format("Expiry Year must be a 4-digit year. Value was \"{0}\"", ExpiryYear.Value));
            }
        }

        private static bool IsWalletPaymentType(PaymentType paymentType)
        {
            return paymentType == PaymentType.ApplePay
                || paymentType == PaymentType.GooglePay
                || paymentType == PaymentType.SamsungPay
                || paymentType == PaymentType.WechatPay
                || paymentType == PaymentType.AmazonPay
                || paymentType == PaymentType.Alipay;
        }

        [JsonProperty(PropertyName = "payment_type")]
        public PaymentType PaymentType { get; set; }

        [JsonProperty(PropertyName = "authorization_id")]
        public string AuthorizationId { get; set; }

        [JsonProperty(PropertyName = "avs_result_code")]
        public string AvsResultCode { get; set; }

        [JsonProperty(PropertyName = "credit_card_company")]
        public string CreditCardCompany { get; set; }

        [JsonProperty(PropertyName = "credit_card_country")]
        public string CreditCardCountry { get; set; }

        [JsonProperty(PropertyName = "credit_card_token")]
        public string CreditCardToken { get; set; }

        [JsonProperty(PropertyName = "cardholder_name")]
        public string CardholderName { get; set; }

        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "stored_payment_created_at")]
        public DateTimeOffset? StoredPaymentCreatedAt { get; set; }

        [JsonProperty(PropertyName = "stored_payment_updated_at")]
        public DateTimeOffset? StoredPaymentUpdatedAt { get; set; }

        [JsonProperty(PropertyName = "installments")]
        public int? Installments { get; set; }

        [JsonProperty(PropertyName = "acquirer_bin")]
        public string AcquirerBin { get; set; }

        [JsonProperty(PropertyName = "acquirer_region")]
        public string AcquirerRegion { get; set; }

        [JsonProperty(PropertyName = "authorization_type")]
        public AuthorizationType? AuthorizationType { get; set; }

        [JsonProperty(PropertyName = "expiry_month")]
        public int? ExpiryMonth { get; set; }

        [JsonProperty(PropertyName = "expiry_year")]
        public int? ExpiryYear { get; set; }

        [JsonProperty(PropertyName = "initial_payment_amount")]
        public float? InitialPaymentAmount { get; set; }

        [JsonProperty(PropertyName = "payment_frequency")]
        public int? PaymentFrequency { get; set; }

        [JsonProperty(PropertyName = "billing_address_id")]
        public string BillingAddressId { get; set; }

        [JsonProperty(PropertyName = "authentication_result")]
        public AuthenticationResult AuthenticationResult { get; set; }
    }
}
