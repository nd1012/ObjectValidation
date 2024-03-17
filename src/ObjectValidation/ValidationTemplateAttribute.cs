using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Template validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="template">Template key</param>
    public class ValidationTemplateAttribute(string template) : ValidationAttributeBase(), IMultipleValidations
    {
        /// <summary>
        /// Template key
        /// </summary>
        public string Template { get; } = template;

        /// <inheritdoc/>
        public virtual IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            if (!ValidationTemplates.PropertyValidations.TryGetValue(Template, out List<ValidationAttribute>? attributes))
            {
                yield return this.CreateValidationResult($"Validation template \"{Template}\" not found", validationContext);
            }
            else
            {
                foreach (ValidationAttribute attr in attributes)
                    if (attr.GetValidationResult(value, validationContext) is ValidationResult result)
                        yield return result;
            }
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => MultiValidation(value, validationContext).FirstOrDefault();
    }
}
