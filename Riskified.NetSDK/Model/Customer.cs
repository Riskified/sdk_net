using System;
using Newtonsoft.Json;
using Riskified.NetSDK.Exceptions;

namespace Riskified.NetSDK.Model
{
    [JsonObject("customer")]
    public class Customer
    {
        /// <summary>
        /// Creates a new Customer
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <param name="firstName">The customer first name</param>
        /// <param name="lastName">The customer last name</param>
        /// <param name="ordersCount">The total number of orders made to the merchant by this customer (optional)</param>
        /// <param name="email">The customer email - as registered with (optional) </param>
        /// <param name="verifiedEmail">Signs if the email was verified by the merchant is some way (optional)</param>
        /// <param name="createdAt">The time of creation of the customer card (optional)</param>
        /// <param name="notes">Additional notes regarding the customer (optional)</param>
        /// <exception cref="OrderFieldBadFormatException">Thrown if one or more of the parameters is missing or of bad format</exception>
        public Customer(int id, string firstName, string lastName, int? ordersCount = null,string email = null, bool? verifiedEmail = null, DateTime? createdAt = null, string notes = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            // optional fields
            if (!string.IsNullOrEmpty(email))
            {
                InputValidators.ValidateEmail(email);
                Email = email;
            }
            OrdersCount = ordersCount;
            VerifiedEmail = verifiedEmail;
            CreatedAt = createdAt;
            Note = notes;
        }

        [JsonProperty(PropertyName = "created_at", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        //todo validate customer ID
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
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
