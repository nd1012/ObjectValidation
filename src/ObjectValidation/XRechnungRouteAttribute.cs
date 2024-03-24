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
    public class XRechnungRouteAttribute(bool normalize = true) : ValidationAttributeBase()
    {
        /// <summary>
        /// Normalize?
        /// </summary>
        public bool Normalize { get; } = normalize;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string route) return this.CreateValidationResult($"XRechnung route value as {typeof(string)} expected", validationContext);
            if (!XRechnungRouting.Validate(Normalize ? XRechnungRouting.Normalize(route) : route)) return this.CreateValidationResult("Invalid XRechnung route value", validationContext);
            return null;
        }
    }
}
