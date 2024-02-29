using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// IBAN validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class IbanAttribute(bool normalize = true) : ValidationAttribute()
    {

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string iban) return this.CreateValidationResult($"IBAN value as {typeof(string)} expected", validationContext);
            if (!SwiftValidation.ValidateIban(Normalize ? SwiftValidation.Normalize(iban) : iban)) return this.CreateValidationResult("Invalid IBAN value", validationContext);
            return null;
        }
    }
}
