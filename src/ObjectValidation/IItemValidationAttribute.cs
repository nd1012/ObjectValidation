using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Interface for an item validation attribute
    /// </summary>
    public interface IItemValidationAttribute : IMultipleValidations
    {
        /// <summary>
        /// Validation target
        /// </summary>
        ItemValidationTargets ValidationTarget { get; }

        /// <summary>
        /// Target array level
        /// </summary>
        int ArrayLevel { get; }
        /// <summary>
        /// Get a validation result for a value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="validationContext">Context (requires the <see cref="System.Reflection.PropertyInfo"/> to be the target object instance (<see cref="ValidationContext.ObjectInstance"/>), and the member name of the value as <see cref="ValidationContext.MemberName"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Result</returns>
        ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider);
    }
}
