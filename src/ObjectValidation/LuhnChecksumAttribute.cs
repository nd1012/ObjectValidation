using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Luhn checksum validation attribute
    /// </summary>
    public class LuhnChecksumAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public LuhnChecksumAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string luhn)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Luhn value as {typeof(string)} expected" : $"{validationContext.MemberName}: Luhn value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!LuhnChecksum.Validate(Normalize ? LuhnChecksum.Normalize(luhn) : luhn))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid Luhn value" : $"{validationContext.MemberName}: Invalid Luhn value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
