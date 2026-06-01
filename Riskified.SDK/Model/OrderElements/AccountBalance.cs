using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// Represents account balance information from a financial data provider.
    /// All fields are required.
    /// </summary>
    public class AccountBalance : IJsonSerializable
    {
        /// <summary>
        /// Creates an AccountBalance instance with all required fields
        /// </summary>
        /// <param name="availableBalance">The available balance in the account</param>
        /// <param name="serviceName">The financial data service provider</param>
        /// <param name="updatedAt">When the balance was last updated (ISO 8601)</param>
        /// <param name="currencyCode">The currency code (ISO 4217)</param>
        public AccountBalance(double availableBalance, AccountBalanceServiceName serviceName, DateTimeOffset updatedAt, string currencyCode)
        {
            AvailableBalance = availableBalance;
            ServiceName = serviceName;
            UpdatedAt = updatedAt;
            CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Validates the object's fields content
        /// </summary>
        /// <param name="validationType">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            if (validationType != Validations.Weak)
            {
                InputValidators.ValidateValuedString(CurrencyCode, "Currency Code");
            }
        }

        [JsonProperty(PropertyName = "available_balance")]
        public double AvailableBalance { get; set; }

        [JsonProperty(PropertyName = "service_name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountBalanceServiceName ServiceName { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "currency_code")]
        public string CurrencyCode { get; set; }
    }
}
