using Newtonsoft.Json;
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
        public enum ECI{
            Zero_Zero = 00,
            Zero_One = 01,
            Zero_Two = 02,
            Zero_Five = 05,
            Zero_Six = 06,
            Zero_Seven = 07

        }

        public enum TranStatus{ 

            Y, N, U, A, C, D, R, I
        }

        public enum TranStatusReason
        {
            Zero_One = 01,
            Zero_TWO = 02,
            Zero_Three = 03,
            Zero_Four = 04,
            Zero_Five = 05,
            Zero_Six = 06,
            Zero_Seven = 07,
            Zero_Eight = 08,
            Zero_Nine = 09,
            Zero_Ten = 10,
            Eleven = 11,
            Twelve = 12,
            Thirteen = 13,
            Fourteen = 14,
            Fifteen = 15,
            Sixteen = 16,
            Seventeen = 17,
            Eighteen = 18,
            Nineteen = 19,
            Twenty = 20,
            Twenty_One = 21,
            Twenty_Two = 22,
            Twenty_Three = 23,
            Twenty_Four = 24,
            Twenty_Five = 25,
            Twenty_Six = 26,
            Twenty_Seven = 27,
            Eighty = 80,
            Ninety_Nine = 99

    }




        public AuthenticationResult(ECI eci)
        {
            this.eci = eci;
        }


        public void Validate(Validations validationType = Validations.Weak)
        {

        }


        [JsonProperty(PropertyName = "eci")]
        public ECI eci { get; set; }

        [JsonProperty(PropertyName = "tran_status")]
        public TranStatus tranStatus { get; set; }

        [JsonProperty(PropertyName = "tran_status_reason")]
        public TranStatusReason tranStatusReason { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "cavv")]
        public string cavv { get; set; }

    }
}
