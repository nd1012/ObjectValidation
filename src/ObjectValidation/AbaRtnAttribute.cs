using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN validation attribute
    /// </summary>
    public class AbaRtnAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public AbaRtnAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

        /// <summary>
        /// Supported ABA RTN formats
        /// </summary>
        public AbaFormats Format { get; set; } = AbaFormats.All;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string aba)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"ABA RTN value as {typeof(string)} expected" : $"{validationContext.MemberName}: ABA RTN value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!AbaValidation.ValidateAbaRtn(Normalize ? AbaValidation.Normalize(aba, Format) : aba))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid ABA RTN value" : $"{validationContext.MemberName}: Invalid ABA RTN value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
