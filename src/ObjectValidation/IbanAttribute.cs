using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// IBAN validation attribute
    /// </summary>
    public class IbanAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public IbanAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

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
