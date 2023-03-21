using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for types or properties which shouldn't be validated
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class NoValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoValidationAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => null;
    }
}
