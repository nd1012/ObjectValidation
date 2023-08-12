using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item template validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ItemValidationTemplateAttribute : Attribute, IItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template">Template key</param>
        /// <param name="target">Validation target</param>
        public ItemValidationTemplateAttribute(string template, ItemValidationTargets target = ItemValidationTargets.Item) : base()
        {
            ValidationTarget = target;
            Template = template;
        }

        /// <inheritdoc/>
        public ItemValidationTargets ValidationTarget { get; }

        /// <inheritdoc/>
        public int ArrayLevel { get; set; }

        /// <summary>
        /// Template key
        /// </summary>
        public string Template { get; }

        /// <inheritdoc/>
        public virtual ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider)
            => MultiValidation(value, validationContext, serviceProvider).FirstOrDefault();

        /// <inheritdoc/>
        public virtual IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            if (!ValidationTemplates.ItemValidations.TryGetValue(Template, out List<IItemValidationAttribute>? attrs))
            {
                yield return new(
                    validationContext.MemberName is null ? $"Validation template \"{Template}\" not found" : $"{validationContext.MemberName}: Validation template \"{Template}\" not found",
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            }
            else
            {
                foreach (IItemValidationAttribute attr in from a in attrs
                                                          where a.ValidationTarget == ValidationTarget &&
                                                            a.ArrayLevel == ArrayLevel
                                                          select a)
                    if (attr.GetValidationResult(value, validationContext, serviceProvider) is ValidationResult result)
                        yield return result;
            }
        }
    }
}
