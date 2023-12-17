using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XRechnung route validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    public class XRechnungRouteAttribute(bool normalize = true) : ValidationAttribute()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string route)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"XRechnung route value as {typeof(string)} expected" : $"{validationContext.MemberName}: XRechnung route value as {typeof(string)} expected"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!XRechnungRouting.Validate(Normalize ? XRechnungRouting.Normalize(route) : route))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid XRechnung route value" : $"{validationContext.MemberName}: Invalid XRechnung route value"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
