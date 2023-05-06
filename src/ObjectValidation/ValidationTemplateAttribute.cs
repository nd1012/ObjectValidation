using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Template validation attribute
    /// </summary>
    public class ValidationTemplateAttribute : ValidationAttribute, IMultipleValidations
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template">Template key</param>
        public ValidationTemplateAttribute(string template) : base() => Template = template;

        /// <summary>
        /// Template key
        /// </summary>
        public string Template { get; }

        /// <inheritdoc/>
        public virtual IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            if (!ValidationTemplates.PropertyValidations.TryGetValue(Template, out List<ValidationAttribute>? attrs))
            {
                yield return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Validation template \"{Template}\" not found" : $"{validationContext.MemberName}: Validation template \"{Template}\" not found"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            }
            else
            {
                foreach (ValidationAttribute attr in attrs)
                    if (attr.GetValidationResult(value, validationContext) is ValidationResult result)
                        yield return result;
            }
        }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) => MultiValidation(value, validationContext).FirstOrDefault();
    }
}
