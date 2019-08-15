using System.Runtime.Serialization;

// This class has been deprecated to support more flexibility with submission reason values. 

namespace Riskified.SDK.Model.OrderElements
{
    public enum SubmissionReason
    {
        [EnumMember(Value = "failed_verification")]
        FailedVerification,
        [EnumMember(Value = "rule_decision")]
        RuleDecision,
        [EnumMember(Value = "third_party")]
        ThirdParty,
        [EnumMember(Value = "manual_decision")]
        ManualDecision,
        [EnumMember(Value = "policy_decision")]
        PolicyDecision
    }
}
