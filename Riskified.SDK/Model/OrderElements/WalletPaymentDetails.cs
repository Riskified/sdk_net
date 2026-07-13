using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
        private static readonly IReadOnlyList<PaymentType> WalletPaymentTypes = new[]
        {
            PaymentType.ApplePay,
            PaymentType.GooglePay,
            PaymentType.SamsungPay,
            PaymentType.WechatPay,
            PaymentType.AmazonPay,
            PaymentType.Alipay
        };

        private static readonly IReadOnlyList<string> ValidAcquirerRegions = new[] { "EU", "NONEU" };

        private sealed class Rule
        {
            public Func<WalletPaymentDetails, bool> Check { get; }
            public string Message { get; }

            public Rule(Func<WalletPaymentDetails, bool> check, string message)
            {
                Check = check;
                Message = message;
            }
        }

        private static readonly IReadOnlyList<Rule> Rules = new List<Rule>
        {
            new Rule(p => WalletPaymentTypes.Contains(p.PaymentType),
                "Payment Type must be one of: " + SupportedPaymentTypes()),
            new Rule(p => !string.IsNullOrEmpty(p.AuthorizationId),
                "Authorization Id can't be null or empty."),
            new Rule(p => !string.IsNullOrEmpty(p.AvsResultCode),
                "AVS Result Code can't be null or empty."),
            new Rule(p => string.IsNullOrEmpty(p.CreditCardCountry) || Country.IsValid(p.CreditCardCountry),
                "Credit Card Country is not a valid ISO country code."),
            new Rule(p => string.IsNullOrEmpty(p.AcquirerRegion) || ValidAcquirerRegions.Contains(p.AcquirerRegion),
                "Acquirer Region must be one of: [" + string.Join(", ", ValidAcquirerRegions) + "]"),
            new Rule(p => !p.ExpiryMonth.HasValue || (p.ExpiryMonth.Value >= 1 && p.ExpiryMonth.Value <= 12),
                "Expiry Month must be between 01 and 12"),
            new Rule(p => !p.ExpiryYear.HasValue || (p.ExpiryYear.Value >= 1900 && p.ExpiryYear.Value <= 9999),
                "Expiry Year must be a 4-digit integer formatted as YYYY")
        };

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
            if (validationType == Validations.Weak) return;

            foreach (var rule in Rules)
            {
                if (!rule.Check(this))
                    throw new OrderFieldBadFormatException(rule.Message);
            }
        }

        private static string SupportedPaymentTypes()
        {
            return "[" + string.Join(", ", WalletPaymentTypes.Select(GetEnumMemberValue)) + "]";
        }

        private static string GetEnumMemberValue(PaymentType type)
        {
            MemberInfo member = typeof(PaymentType).GetMember(type.ToString()).FirstOrDefault();
            EnumMemberAttribute attr = member?.GetCustomAttribute<EnumMemberAttribute>();
            return attr != null ? attr.Value : type.ToString().ToLower();
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
