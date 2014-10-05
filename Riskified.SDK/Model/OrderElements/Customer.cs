using System;
using Newtonsoft.Json;
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
        /// <exception cref="OrderFieldBadFormatException">Thrown if one or more of the parameters is missing or of bad format</exception>
        public Customer(string firstName, string lastName,int? id, int? ordersCount = null,string email = null, bool? verifiedEmail = null, DateTime? createdAt = null, string notes = null)
        {
            FirstName = firstName;
            LastName = lastName;
            // optional fields
            Id = id;
            Email = email;
            OrdersCount = ordersCount;
            VerifiedEmail = verifiedEmail;
            CreatedAt = createdAt;
            Note = notes;
        }

        public void Validate(bool isWeak=false)
        {
            InputValidators.ValidateValuedString(FirstName, "First Name");
            InputValidators.ValidateValuedString(LastName, "Last Name");
            if (!string.IsNullOrEmpty(Email))
            {
                InputValidators.ValidateEmail(Email);
            }
            if (CreatedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            }
        }

        [JsonProperty(PropertyName = "created_at", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "last_name", Required = Required.Always)]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "note", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        [JsonProperty(PropertyName = "orders_count", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public int? OrdersCount { get; set; }

        [JsonProperty(PropertyName = "verified_email",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public bool? VerifiedEmail { get; set; }

        
    }
}
