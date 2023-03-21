using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// European VAT ID validation attribute
    /// </summary>
    public class EuVatIdAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public EuVatIdAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            if (value is not string vatId)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"VAT ID value as {typeof(string)} expected" : $"{validationContext.MemberName}: VAT ID value as {typeof(string)} expected"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (!EuVatId.Validate(Normalize ? EuVatId.Normalize(vatId) : vatId))
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid VAT ID value" : $"{validationContext.MemberName}: Invalid VAT ID value"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
