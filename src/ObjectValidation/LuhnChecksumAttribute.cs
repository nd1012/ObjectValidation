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
            if (value == null) return null;
            if (value is not string luhn)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Luhn value as {typeof(string)} expected" : $"{validationContext.MemberName}: Luhn value as {typeof(string)} expected"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (!LuhnChecksum.Validate(Normalize ? LuhnChecksum.Normalize(luhn) : luhn))
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid Luhn value" : $"{validationContext.MemberName}: Invalid Luhn value"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
