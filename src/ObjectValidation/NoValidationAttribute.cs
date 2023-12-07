using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for types or properties which shouldn't be validated (optional completely)
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="skipNullValueCheck">Skip the <see langword="null"/> value check(s)?</param>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class NoValidationAttribute(bool skipNullValueCheck = false) : ValidationAttribute()
    {
        /// <summary>
        /// Skip the <see langword="null"/> value check(s)?
        /// </summary>
        public bool SkipNullValueCheck { get; } = skipNullValueCheck;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => null;
    }
}
