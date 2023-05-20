using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for types or properties which shouldn't be validated (optional completely)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class NoValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="skipNullValueCheck">Skip the <see langword="null"/> value check(s)?</param>
        public NoValidationAttribute(bool skipNullValueCheck = false) : base() => SkipNullValueCheck = skipNullValueCheck;

        /// <summary>
        /// Skip the <see langword="null"/> value check(s)?
        /// </summary>
        public bool SkipNullValueCheck { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => null;
    }
}
