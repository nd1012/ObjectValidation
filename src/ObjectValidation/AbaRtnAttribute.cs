using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class AbaRtnAttribute(bool normalize = true) : ValidationAttribute()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <summary>
        /// Supported ABA RTN formats
        /// </summary>
        public AbaFormats Format { get; set; } = AbaFormats.All;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string aba) return this.CreateValidationResult($"ABA RTN value as {typeof(string)} expected", validationContext);
            if (!AbaValidation.ValidateAbaRtn(Normalize ? AbaValidation.Normalize(aba, Format) : aba)) return this.CreateValidationResult("Invalid ABA RTN value", validationContext);
            return null;
        }
    }
}
