using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Conditional item template validation attribute
    /// </summary>
    public class ItemValidationTemplateIfAttribute : ItemValidationTemplateAttribute
    {
        /// <summary>
        /// Property
        /// </summary>
        protected PropertyInfo? Property = null;
        /// <summary>
        /// Property getter
        /// </summary>
        protected ReflectionHelper.PropertyGetter_Delegate? PropertyGetter = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template">Template key</param>
        /// <param name="propertyName">Checked property name</param>
        /// <param name="target">Validation target</param>
        /// <param name="values">Values (one of the value is required in order to match this condition; if not given, the temlate will be applied if the checked property has a non-
        /// <see langword="null"/> value)</param>
        public ItemValidationTemplateIfAttribute(string template, string propertyName, ItemValidationTargets target = ItemValidationTargets.Item, params object?[] values)
            : base(template, target)
        {
            PropertyName = propertyName;
            Values = values;
        }

        /// <summary>
        /// Checked property name
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Expected values
        /// </summary>
        public object?[] Values { get; }

        /// <summary>
        /// Invert the checked property values meaning (the template will be applied, if the checked property value is NOT in the value list)
        /// </summary>
        public bool IfNotInValues { get; set; }

        /// <summary>
        /// Invert the checked property value requirement, if no values were given (the template will be applied, if the checked property value IS <see langword="null"/>)
        /// </summary>
        public bool ApplyIfNull { get; set; }

        /// <inheritdoc/>
        public override IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            if (PropertyGetter is null)
            {
                Property = validationContext.ObjectInstance.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                    ?? throw new InvalidDataException($"Property {PropertyName} not found in validated type {validationContext.ObjectInstance.GetType()}");
                PropertyGetter = Property.GetGetterDelegate()
                    ?? throw new InvalidDataException($"Property {validationContext.ObjectInstance.GetType()}.{Property.Name} requires a public getter");
            }
            object? v = PropertyGetter(validationContext.ObjectInstance);
            if ((Values.Length > 0 && IfNotInValues != Values.Contains(v)) || (Values.Length == 0 && ApplyIfNull == (v is null)))
                foreach (ValidationResult result in base.MultiValidation(value, validationContext, serviceProvider))
                    yield return result;
        }
    }
}
