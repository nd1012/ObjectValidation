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
    public class AmountAttribute(string currency) : ValidationAttributeBase()
    {
        /// <summary>
        /// Currency ISO 4217 code
        /// </summary>
        public string Currency { get; } = currency;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not decimal amount) return this.CreateValidationResult($"Amount value as {typeof(decimal)} expected", validationContext);
            if (!CurrencyCodes.Known.TryGetValue(Currency, out CurrencyCodes.Currency? v)) return this.CreateValidationResult("Invalid currency configured", validationContext);
            if (!v.Validate(amount)) return this.CreateValidationResult("Invalid amount value", validationContext);
            return null;
        }
    }
}
