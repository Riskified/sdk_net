﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.AccountActionElements
{
    public class Login : AbstractAccountAction
    {
        public Login(string customerId, string email, LoginStatus loginStatus, ClientDetails clientDetails, SessionDetails sessionDetails) :
        base(customerId, clientDetails, sessionDetails)
        {
            Email = email;
            LoginStatus = loginStatus;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "customer_created_at")]
        public DateTime? CustomerCreatedAt { get; set; }

        [JsonProperty(PropertyName = "login_status")]
        public LoginStatus LoginStatus { get; set; }

        [JsonProperty(PropertyName = "login_at_checkout")]
        public bool? LoginAtCheckout { get; set; }

        [JsonProperty(PropertyName = "social_login_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SocialType? SocialLoginType { get; set; }

        [JsonProperty(PropertyName = "challenge_redirect_url")]
        public string ChallengeRedirectUrl { get; set; }

        [JsonProperty(PropertyName = "account_recovery_url")]
        public string AccountRecoveryUrl { get; set; }
    }
}