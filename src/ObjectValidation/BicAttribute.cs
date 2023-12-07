using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// BIC validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class BicAttribute(bool normalize = true) : ValidationAttribute()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string bic)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"BIC value as {typeof(string)} expected" : $"{validationContext.MemberName}: BIC value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!SwiftValidation.ValidateIban(Normalize ? SwiftValidation.Normalize(bic) : bic))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid BIC value" : $"{validationContext.MemberName}: Invalid BIC value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
