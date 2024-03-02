using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// European VAT ID validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class EuVatIdAttribute(bool normalize = true) : ValidationAttribute()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string vatId) return this.CreateValidationResult($"VAT ID value as {typeof(string)} expected", validationContext);
            if (!EuVatId.Validate(Normalize ? EuVatId.Normalize(vatId) : vatId)) return this.CreateValidationResult("Invalid VAT ID value", validationContext);
            return null;
        }
    }
}
