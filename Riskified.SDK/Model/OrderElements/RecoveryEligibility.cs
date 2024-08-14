using System;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.OrderElements
{
    public class RecoveryEligibility : IJsonSerializable
    {
        public RecoveryEligibility(string id, string status, string verifiedAt, string challengeType)
        {
            Id = id;
            Status = status;
            VerifiedAt = verifiedAt;
            ChallengeType = challengeType;

        }

        public void Validate(Validations validationType = Validations.Weak)
        {
            throw new NotImplementedException();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "verified_at")]
        public string VerifiedAt { get; set; }

        [JsonProperty(PropertyName = "challenge_type")]
        public string ChallengeType { get; set; }
    }
}
