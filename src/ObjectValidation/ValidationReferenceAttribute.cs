using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute to reference another property for inheriting its validation attributes during a validation is being executed
    /// </summary>
    public class ValidationReferenceAttribute : ValidationAttribute, IMultipleValidations
    {
        /// <summary>
        /// Validation attributes
        /// </summary>
        protected readonly ValidationAttribute[] Attributes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Referenced type</param>
        /// <param name="propertyName">Referenced property name</param>
        public ValidationReferenceAttribute(Type type, string propertyName) : base()
        {
            ReferencedProperty = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new ArgumentException($"Public instance property \"{type}.{propertyName}\" not found", nameof(propertyName));
            Attributes = ReferencedProperty.GetCustomAttributesCached().Where(a => a is ValidationAttribute).Cast<ValidationAttribute>().ToArray();
        }

        /// <summary>
        /// Referenced property
        /// </summary>
        public PropertyInfo ReferencedProperty { get; }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            foreach (ValidationAttribute attr in Attributes)
                if (attr.GetValidationResult(value, validationContext) is ValidationResult result)
                    yield return result;
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => MultiValidation(value, validationContext).FirstOrDefault();
    }
}
