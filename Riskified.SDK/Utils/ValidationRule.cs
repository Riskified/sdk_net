using System;
using System.Collections.Generic;
using Riskified.SDK.Exceptions;

namespace Riskified.SDK.Utils
{
    /// <summary>
    /// A single validation rule: a predicate over <typeparamref name="T"/> paired with the error
    /// message reported when the predicate returns false. Lets models declare their sequential
    /// validation as data instead of an imperative check block.
    /// </summary>
    internal sealed class ValidationRule<T>
    {
        public Func<T, bool> Check { get; }
        public string Message { get; }

        public ValidationRule(Func<T, bool> check, string message)
        {
            Check = check;
            Message = message;
        }
    }

    internal static class ValidationRuleExtensions
    {
        /// <summary>
        /// Runs each rule against <paramref name="target"/> in order, throwing
        /// <see cref="OrderFieldBadFormatException"/> with the rule's message on the first failure.
        /// </summary>
        public static void RunAll<T>(this IEnumerable<ValidationRule<T>> rules, T target)
        {
            foreach (var rule in rules)
            {
                if (!rule.Check(target))
                    throw new OrderFieldBadFormatException(rule.Message);
            }
        }
    }
}
