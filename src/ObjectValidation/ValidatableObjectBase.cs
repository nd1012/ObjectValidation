using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for an object validatable object
    /// </summary>
    public abstract class ValidatableObjectBase : IObjectValidatable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ValidatableObjectBase() { }

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
        public static implicit operator ValidationResult[](ValidatableObjectBase obj) => obj.Validate(new(obj, serviceProvider: null, items: null)).ToArray();

        /// <summary>
        /// Make an object <see cref="ValidatableObjectBase"/> compatible (execute in the <see cref="IValidatableObject.Validate(ValidationContext)"/> method)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Validation results</returns>
        public static IEnumerable<ValidationResult> ObjectValidatable(IObjectValidatable obj)
        {
            List<ValidationResult> res = [];
            ValidationExtensions.TryValidateObject(obj, res);
            return res;
        }
    }
}
