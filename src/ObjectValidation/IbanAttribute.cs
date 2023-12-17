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
            if (value is not string iban)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"IBAN value as {typeof(string)} expected" : $"{validationContext.MemberName}: IBAN value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!SwiftValidation.ValidateIban(Normalize ? SwiftValidation.Normalize(iban) : iban))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid IBAN value" : $"{validationContext.MemberName}: Invalid IBAN value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
