using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for an object validatable object
    /// </summary>
    public abstract class ValidatableObject : IObjectValidatable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ValidatableObject() { }

        /// <summary>
        /// Validate the object
        /// </summary>
        /// <param name="validationContext">Context</param>
        /// <returns>Results</returns>
        protected virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => ObjectValidatable(this);

        /// <inheritdoc/>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext) => Validate(validationContext);

        /// <summary>
        /// Get validation results
        /// </summary>
        /// <param name="obj">Object</param>
        public static implicit operator ValidationResult[](ValidatableObject obj) => obj.Validate(new(obj, serviceProvider: null, items: null)).ToArray();

        /// <summary>
        /// Make an object <see cref="ValidatableObject"/> compatible (execute in the <see cref="IValidatableObject.Validate(ValidationContext)"/> method)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Validation results</returns>
        public static IEnumerable<ValidationResult> ObjectValidatable(IObjectValidatable obj)
        {
            List<ValidationResult> res = new();
            ValidationExtensions.TryValidateObject(obj, res);
            return res;
        }
    }
}
