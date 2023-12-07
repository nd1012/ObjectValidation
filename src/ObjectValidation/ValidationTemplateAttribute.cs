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
    public class ValidationTemplateAttribute(string template) : ValidationAttribute(), IMultipleValidations
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
                yield return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Validation template \"{Template}\" not found" : $"{validationContext.MemberName}: Validation template \"{Template}\" not found"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
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
