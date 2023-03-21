using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// BIC validation attribute
    /// </summary>
    public class BicAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public BicAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            if (value is not string bic)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"BIC value as {typeof(string)} expected" : $"{validationContext.MemberName}: BIC value as {typeof(string)} expected"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (!SwiftValidation.ValidateIban(Normalize ? SwiftValidation.Normalize(bic) : bic))
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid BIC value" : $"{validationContext.MemberName}: Invalid BIC value"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
