using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Luhn checksum validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class LuhnChecksumAttribute(bool normalize = true) : ValidationAttributeBase()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string luhn) return this.CreateValidationResult($"Luhn value as {typeof(string)} expected", validationContext);
            if (!LuhnChecksum.Validate(Normalize ? LuhnChecksum.Normalize(luhn) : luhn)) return this.CreateValidationResult("Invalid Luhn value", validationContext);
            return null;
        }
    }
}
