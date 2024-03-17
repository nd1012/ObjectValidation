using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Country code validation attribute
    /// </summary>
    public class CountryAttribute : ValidationAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CountryAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string country) return this.CreateValidationResult($"Country code value as {typeof(string)} expected", validationContext);
            if (!CountryCodes.Known.ContainsKey(country)) return this.CreateValidationResult("Invalid country code value", validationContext);
            return null;
        }
    }
}
