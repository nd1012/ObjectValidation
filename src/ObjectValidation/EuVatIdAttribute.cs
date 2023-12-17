using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// European VAT ID validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class EuVatIdAttribute(bool normalize = true) : ValidationAttribute()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string vatId)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"VAT ID value as {typeof(string)} expected" : $"{validationContext.MemberName}: VAT ID value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!EuVatId.Validate(Normalize ? EuVatId.Normalize(vatId) : vatId))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid VAT ID value" : $"{validationContext.MemberName}: Invalid VAT ID value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
