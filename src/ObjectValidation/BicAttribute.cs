using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// BIC validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class BicAttribute(bool normalize = true) : ValidationAttributeBase()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string bic) return this.CreateValidationResult($"BIC value as {typeof(string)} expected", validationContext);
            if (!SwiftValidation.ValidateIban(Normalize ? SwiftValidation.Normalize(bic) : bic)) return this.CreateValidationResult("Invalid BIC value", validationContext);
            return null;
        }
    }
}
