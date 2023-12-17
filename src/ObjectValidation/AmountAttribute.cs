using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Amount validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="currency">Currency ISO 4217 code</param>
    public class AmountAttribute(string currency) : ValidationAttribute()
    {
        /// <summary>
        /// Currency ISO 4217 code
        /// </summary>
        public string Currency { get; } = currency;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not decimal amount)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Amount value as {typeof(decimal)} expected" : $"{validationContext.MemberName}: Amount value as {typeof(decimal)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!CurrencyCodes.Known.TryGetValue(Currency, out CurrencyCodes.Currency? v))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid currency configured" : $"{validationContext.MemberName}: Invalid currency configured"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!v.Validate(amount))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid amount value" : $"{validationContext.MemberName}: Invalid amount value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
