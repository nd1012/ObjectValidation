using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item template validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="template">Template key</param>
    /// <param name="target">Validation target</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ItemValidationTemplateAttribute(string template, ItemValidationTargets target = ItemValidationTargets.Item) : Attribute(), IItemValidationAttribute
    {
        /// <inheritdoc/>
        public ItemValidationTargets ValidationTarget { get; } = target;

        /// <inheritdoc/>
        public int ArrayLevel { get; set; }

        /// <summary>
        /// Template key
        /// </summary>
        public string Template { get; } = template;

        /// <inheritdoc/>
        public virtual ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider)
            => MultiValidation(value, validationContext, serviceProvider).FirstOrDefault();

        /// <inheritdoc/>
        public virtual IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            if (!ValidationTemplates.ItemValidations.TryGetValue(Template, out List<IItemValidationAttribute>? attributes))
            {
                yield return new(
                    validationContext.MemberName is null ? $"Validation template \"{Template}\" not found" : $"{validationContext.MemberName}: Validation template \"{Template}\" not found",
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            }
            else
            {
                foreach (IItemValidationAttribute attr in from a in attributes
                                                          where a.ValidationTarget == ValidationTarget &&
                                                            a.ArrayLevel == ArrayLevel
                                                          select a)
                    if (attr.GetValidationResult(value, validationContext, serviceProvider) is ValidationResult result)
                        yield return result;
            }
        }
    }
}
