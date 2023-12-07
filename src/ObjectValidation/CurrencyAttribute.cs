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
            if (value is not string currency)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Currency code value as {typeof(string)} expected" : $"{validationContext.MemberName}: Currency code value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            switch (Format)
            {
                case CurrencyFormats.AlphabeticCode:
                    if (!CurrencyCodes.Known.ContainsKey(currency))
                        return new(
                            ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid currency code value" : $"{validationContext.MemberName}: Invalid currency code value"),
                            validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                            );
                    break;
                case CurrencyFormats.NumericCode:
                    if (!CurrencyCodes.Known.Values.Any(c => c.NumericCode == currency))
                        return new(
                            ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid currency code value" : $"{validationContext.MemberName}: Invalid currency code value"),
                            validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                            );
                    break;
                default:
                    return new(
                        ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid currency format configured" : $"{validationContext.MemberName}: Invalid currency format configured"),
                        validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                        );
            }
            return null;
        }
    }
}
