using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Country code validation attribute
    /// </summary>
    public class CountryAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CountryAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            if (value is not string country)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Country code value as {typeof(string)} expected" : $"{validationContext.MemberName}: Country code value as {typeof(string)} expected"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            if (!CountryCodes.Known.ContainsKey(country))
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid country code value" : $"{validationContext.MemberName}: Invalid country code value"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
