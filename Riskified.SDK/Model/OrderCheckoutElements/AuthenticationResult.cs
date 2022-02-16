﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model.OrderCheckoutElements
{
    public class AuthenticationResult : IJsonSerializable
    {
        public enum TranStatus
        {
            Y, N, U, A, C, D, R, I
        }

        public AuthenticationResult(string eci)
        {
            this.eci = eci;
        }


        public void Validate(Validations validationType = Validations.Weak)
        {

        }


        [JsonProperty(PropertyName = "eci")]
        public string eci { get; set; }

        [JsonProperty(PropertyName = "tran_status")]
        public TranStatus tranStatus { get; set; }

        [JsonProperty(PropertyName = "tran_status_reason")]
        public string tranStatusReason { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "liability_shift")]
        public bool LiabilityShift { get; set; }

        [JsonProperty(PropertyName = "cavv")]
        public string cavv { get; set; }

        [JsonProperty(PropertyName = "three_d_challenge")]
        public bool ThreeDChallenge { get; set; }

        [JsonProperty(PropertyName = "TRA_exemption")]
        public bool TraExemption { get; set; }

    }
}

