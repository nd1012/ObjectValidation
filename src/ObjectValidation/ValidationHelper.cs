using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Object validation helper
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Create a validation result
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="validationContext">Context</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult CreateValidationResult(in string message, in ValidationContext validationContext)
            => new(
                validationContext.MemberName is null
                    ? message
                    : $"{validationContext.MemberName}: {message}",
                validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                );
    }
}
