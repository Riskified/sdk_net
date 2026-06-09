using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum AiAgent
    {
        [EnumMember(Value = "chatgpt")]
        Chatgpt,
        [EnumMember(Value = "gemini")]
        Gemini,
        [EnumMember(Value = "copilot")]
        Copilot,
        [EnumMember(Value = "perplexity")]
        Perplexity
    }
}
