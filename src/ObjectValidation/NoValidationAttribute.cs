using System.ComponentModel.DataAnnotations;

//TODO Optional skip NULL value check

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
        public NoValidationAttribute() : base() { }
        
        /// <summary>
        /// Skip the <see langword="null"/> value check(s)?
        /// </summary>
        public bool SkipNullValueCheck { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => null;
    }
}
