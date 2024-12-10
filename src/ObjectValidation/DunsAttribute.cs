using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// D-U-N-S number validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class DunsAttribute(bool normalize = true) : ValidationAttributeBase()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string duns) return this.CreateValidationResult($"D-U-N-S number value as {typeof(string)} expected", validationContext);
            if (!DunsValidation.ValidateDuns(Normalize ? DunsValidation.Normalize(duns) : duns)) return this.CreateValidationResult("Invalid D-U-N-S number value", validationContext);
            return null;
        }
    }
}
