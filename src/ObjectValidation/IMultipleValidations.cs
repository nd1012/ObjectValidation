using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Interface for validation attributes which perform multiple validations (and therefor may return multiple validation results)
    /// </summary>
    public interface IMultipleValidations
    {
        /// <summary>
        /// Perform multiple validations
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="validationContext">Context</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        public IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null);
    }
}
