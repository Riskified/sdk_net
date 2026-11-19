using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Exceptions;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    [JsonObject("customer")]
    public class Customer : IJsonSerializable
    {
        /// <summary>
        /// Creates a new Customer
        /// </summary>
        /// <param name="firstName">The customer first name</param>
        /// <param name="lastName">The customer last name</param>
        /// <param name="id">The customer id (optional)</param>
        /// <param name="ordersCount">The total number of orders made to the merchant by this customer (optional)</param>
        /// <param name="email">The customer email - as registered with (optional) </param>
        /// <param name="verifiedEmail">Signs if the email was verified by the merchant is some way (optional)</param>
        /// <param name="createdAt">The time of creation of the customer card (optional)</param>
        /// <param name="notes">Additional notes regarding the customer (optional)</param>
        public Customer(string firstName, string lastName, string id, int? ordersCount = null, string email = null, bool? verifiedEmail = null, DateTimeOffset? verifiedEmailAt = null, bool? verifiedPhone = null, DateTimeOffset? verifiedPhoneAt = null, DateTimeOffset? createdAt = null, DateTimeOffset? updatedAt = null,  string notes = null, SocialDetails[] social = null, BasicAddress address = null, string accountType = null, int? linkedAccounts = null, DateTimeOffset? firstPurchaseAt = null, string documentId = null, string documentType = null, float? totalSpent = null)
        {
            FirstName = firstName;
            LastName = lastName;

            // optional fields
            Id = id;
            Email = email;
            OrdersCount = ordersCount;
            VerifiedEmail = verifiedEmail;
            VerifiedEmailAt = verifiedEmailAt;
            VerifiedPhone = verifiedPhone;
            VerifiedPhoneAt = verifiedPhoneAt;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Note = notes;
            Social = social;
            Address = address;
            AccountType = accountType;
            LinkedAccounts = linkedAccounts;
            FirstPurchaseAt = firstPurchaseAt;
            DocumentId = documentId;
            DocumentType = documentType;
            TotalSpent = totalSpent;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="validationType">Validation level to use on this Model</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public void Validate(Validations validationType = Validations.Weak)
        {
            if (validationType == Validations.Weak)
            {
                if(string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                {
                    throw new Exceptions.OrderFieldBadFormatException("Both First name and last name are missing or empty - at least one should be specified");
                }
            }
            else
            {
                InputValidators.ValidateValuedString(FirstName, "First Name");
                InputValidators.ValidateValuedString(LastName, "Last Name");
            }

            // optional fields validations
            if (!string.IsNullOrEmpty(Email))
            {
                InputValidators.ValidateEmail(Email);
            }
            if (OrdersCount.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(OrdersCount.Value, "Orders Count");
            }
            if (CreatedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            }
            if (UpdatedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(UpdatedAt.Value, "Updated At");
            }
            if (VerifiedEmailAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(VerifiedEmailAt.Value, "Verified Email At");
            }
            if (VerifiedPhoneAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(VerifiedPhoneAt.Value, "Verified Phone At");
            }
            if (FirstPurchaseAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(FirstPurchaseAt.Value, "First Purchase At");
            }
            if (TotalSpent.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalSpent.Value, "Total Spent");
            }
            if (Social != null)
            {
                Social.ToList().ForEach(item => item.Validate(validationType));
            }
            if (Address != null)
            {
                Address.Validate(validationType);
            }
        }

        [JsonProperty(PropertyName = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "orders_count")]
        public int? OrdersCount { get; set; }

        [JsonProperty(PropertyName = "total_spent")]
        public float? TotalSpent { get; set; }

        [JsonProperty(PropertyName = "verified_email")]
        public bool? VerifiedEmail { get; set; }

        [JsonProperty(PropertyName = "verified_email_at")]
        public DateTimeOffset? VerifiedEmailAt { get; set; }

        [JsonProperty(PropertyName = "verified_phone")]
        public bool? VerifiedPhone { get; set; }

        [JsonProperty(PropertyName = "verified_phone_at")]
        public DateTimeOffset? VerifiedPhoneAt { get; set; }

        [JsonProperty(PropertyName = "social")]
        public SocialDetails[] Social { get; set; }

        [JsonProperty(PropertyName = "buyer_anonymous_id")]
        public string AnonymousId { get; set; }

        [JsonProperty(PropertyName = "address")]
        public BasicAddress Address { get; set; }
        
        [JsonProperty(PropertyName = "account_type")]
        public string AccountType { get; set; }

        [JsonProperty(PropertyName = "linked_accounts")]
        public int? LinkedAccounts { get; set; }

        [JsonProperty(PropertyName = "first_purchase_at")]
        public DateTimeOffset? FirstPurchaseAt { get; set; }

        [JsonProperty(PropertyName = "document_type")]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "document_id")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "spending_limit")]
        public float? SpendingLimit { get; set; }

        [JsonProperty(PropertyName = "has_defaulted")]
        public bool? HasDefaulted { get; set; }

        [JsonProperty(PropertyName = "date_of_birth")]
        public DateTimeOffset? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "kyc_details")]
        public KycDetails[] KycDetails { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Any tags related to the customer in the merchant's system.
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }

        /// <summary>
        /// Required when the site user name isn't the email address.
        /// </summary>
        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// The name of the group the customer belongs to in the merchant's system.
        /// </summary>
        [JsonProperty(PropertyName = "group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// The ID of the last order the customer purchased.
        /// </summary>
        [JsonProperty(PropertyName = "last_order_id")]
        public string LastOrderId { get; set; }

        /// <summary>
        /// The customer ID of the customer who referred this customer to the merchant's website.
        /// </summary>
        [JsonProperty(PropertyName = "referrer_customer_id")]
        public string ReferrerCustomerId { get; set; }

        [JsonProperty(PropertyName = "accepts_marketing")]
        public bool? AcceptsMarketing { get; set; }

        /// <summary>
        /// The outcome of a KYC check.
        /// </summary>
        [JsonProperty(PropertyName = "kyc_verified")]
        public bool? KycVerified { get; set; }

        /// <summary>
        /// Whether a phone number was a required field during registration.
        /// </summary>
        [JsonProperty(PropertyName = "phone_mandatory")]
        public bool? PhoneMandatory { get; set; }

        /// <summary>
        /// The number of attempts the customer made to buy an item in the merchant's system.
        /// </summary>
        [JsonProperty(PropertyName = "buy_attempts")]
        public int? BuyAttempts { get; set; }

        /// <summary>
        /// The number of attempts the customer made to sell an item in the merchant's system.
        /// </summary>
        [JsonProperty(PropertyName = "sell_attempts")]
        public int? SellAttempts { get; set; }

        [JsonProperty(PropertyName = "gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }

        /// <summary>
        /// The social network the customer signed up with, if any.
        /// </summary>
        [JsonProperty(PropertyName = "social_signup_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SocialType? SocialSignupType { get; set; }

        /// <summary>
        /// The crypto wallets associated with the customer's account.
        /// </summary>
        [JsonProperty(PropertyName = "crypto_wallets")]
        public CryptoWallet[] CryptoWallets { get; set; }
    }
}
