using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Count limitation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CountLimitAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="max">Maximum</param>
        public CountLimitAttribute(long max) : base() => Max = max;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        public CountLimitAttribute(long min, long max) : base()
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Minimum
        /// </summary>
        public long? Min { get; }

        /// <summary>
        /// Maximum
        /// </summary>
        public long Max { get; }

        /// <summary>
        /// Get the error message
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="member">Member</param>
        /// <returns>Error message</returns>
        public string? GetErrorMessage(long? count, string? member)
            => count == null || ((Min == null || count >= Min) && count <= Max)
                ? null
                : member == null
                        ? (Min == null
                            ? ErrorMessage ?? $"Maximum count is {Max} ({count})"
                            : ErrorMessage ?? $"Count must be between {Min} and {Max} ({count})")
                        : (Min == null
                            ? (ErrorMessage == null
                                ? $"Maximum count of the property {member} is {Max} ({count})"
                                : $"{member}: {ErrorMessage}")
                            : (ErrorMessage == null
                                ? $"Count of the property {member} must be between {Min} and {Max} ({count})"
                                : $"{member}: {ErrorMessage}"));

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            => GetErrorMessage(
                    value switch
                    {
                        ILongCountable lca => lca.LongCount,
                        ICountable ca => ca.Count,
                        IDictionary dict => dict.Count,
                        Array arr => arr.LongLength,
                        IList list => list.Count,
                        ICollection col => col.Count,
                        _ => null
                    },
                    validationContext.MemberName
                    ) is string error
                ? new ValidationResult(error, validationContext.MemberName == null ? null : new string[] { validationContext.MemberName })
                : null;
    }
}
