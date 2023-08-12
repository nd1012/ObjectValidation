using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XRechnung route validation attribute
    /// </summary>
    public class XRechnungRouteAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        public XRechnungRouteAttribute(bool normalize = true) : base() => Normalize = normalize;

        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; }

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
