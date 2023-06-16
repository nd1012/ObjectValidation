using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for a conditional value requirement validation
    /// </summary>
    public class RequiredIfAttribute : RequiredAttribute
    {
        /// <summary>
        /// Property
        /// </summary>
        protected PropertyInfo? Property = null;
        /// <summary>
        /// Property getter
        /// </summary>
        protected Func<object?, object?>? PropertyGetter = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">Checked property name</param>
        /// <param name="values">Values (one of the value is required in order to match this condition; if not given, the value is required if the checked property has a non-
        /// <see langword="null"/> value)</param>
        public RequiredIfAttribute(string propertyName, params object?[] values) : base()
        {
            PropertyName = propertyName;
            Values = values;
        }

        /// <summary>
        /// Checked property name
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Checked property values (one of the value is required in order to match this condition; if not given, the value is required if the checked property has a non-
        /// <see langword="null"/> value)
        /// </summary>
        public object?[] Values { get; }

        /// <summary>
        /// Invert the checked property values meaning (this property needs to have a value, if the checked property value is NOT in the value list)
        /// </summary>
        public bool IfNotInValues { get; set; }

        /// <summary>
        /// Invert the checked property value requirement, if no values were given (this property is required, if the checked property value IS <see langword="null"/>)
        /// </summary>
        public bool RequiredIfNull { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (PropertyGetter == null)
            {
                Property = validationContext.ObjectInstance.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                    ?? throw new InvalidDataException($"Property {PropertyName} not found in validated type {validationContext.ObjectInstance.GetType()}");
                PropertyGetter = Property.GetGetterDelegate()
                    ?? throw new InvalidDataException($"Property {validationContext.ObjectInstance.GetType()}.{Property.Name} requires a public getter");
            }
            object? v = PropertyGetter(validationContext.ObjectInstance);
            return (Values.Length > 0 && IfNotInValues != Values.Contains(v)) || (Values.Length == 0 && RequiredIfNull == (v == null))
                ? base.IsValid(value, validationContext)
                : null;
        }
    }
}
