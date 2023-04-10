using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for an object validatable object
    /// </summary>
    public abstract class ValidatableObject : IObjectValidatable, IEnumerable<ValidationResult>
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
        IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator() => Validate(new(this, serviceProvider: null, items: null)).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => Validate(new(this, serviceProvider: null, items: null)).GetEnumerator();

        /// <inheritdoc/>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext) => Validate(validationContext);

        /// <summary>
        /// Get validation results
        /// </summary>
        /// <param name="obj">Object</param>
        public static implicit operator ValidationResult[](ValidatableObject obj) => obj.Validate(new(obj, serviceProvider: null, items: null)).ToArray();
    }
}
