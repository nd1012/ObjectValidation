using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Amount validation attribute
    /// </summary>
    public class AmountAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currency">Currency ISO 4217 code</param>
        public AmountAttribute(string currency) : base() => Currency = currency;

        /// <summary>
        /// Currency ISO 4217 code
        /// </summary>
        public string Currency { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not decimal amount)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Amount value as {typeof(decimal)} expected" : $"{validationContext.MemberName}: Amount value as {typeof(decimal)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!CurrencyCodes.Known.ContainsKey(Currency))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid currency configured" : $"{validationContext.MemberName}: Invalid currency configured"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!CurrencyCodes.Known[Currency].Validate(amount))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid amount value" : $"{validationContext.MemberName}: Invalid amount value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
