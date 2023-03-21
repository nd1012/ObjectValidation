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
        protected ValidatableObject() : base() { }

        /// <summary>
        /// Validate the object
        /// </summary>
        /// <param name="validationContext">Context</param>
        /// <returns>Results</returns>
        protected virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new();
            ValidationExtensions.TryValidateObject(this, res);
            return res;
        }

        /// <inheritdoc/>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext) => Validate(validationContext);
    }
}
