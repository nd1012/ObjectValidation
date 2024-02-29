using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Currency ISO 4217 code validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="format">Format</param>
    public class CurrencyAttribute(CurrencyFormats format = CurrencyFormats.AlphabeticCode) : ValidationAttribute()
    {
        /// <summary>
        /// Format
        /// </summary>
        public CurrencyFormats Format { get; } = format;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string currency) return this.CreateValidationResult($"Currency code value as {typeof(string)} expected", validationContext);
            switch (Format)
            {
                case CurrencyFormats.AlphabeticCode:
                    if (!CurrencyCodes.Known.ContainsKey(currency)) return this.CreateValidationResult("Invalid currency code value", validationContext);
                    break;
                case CurrencyFormats.NumericCode:
                    if (!CurrencyCodes.Known.Values.Any(c => c.NumericCode == currency)) return this.CreateValidationResult("Invalid currency code value", validationContext);
                    break;
                default:
                    return this.CreateValidationResult("Invalid currency format configured", validationContext);
            }
            return null;
        }
    }
}
