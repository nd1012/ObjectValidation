﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for a conditional value requirement validation
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="propertyName">Checked property name</param>
    /// <param name="values">Values (one of the value is required in order to match this condition; if not given, the value is required if the checked property has a non-
    /// <see langword="null"/> value)</param>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RequiredIfAttribute(string propertyName, params object?[] values) : RequiredAttribute()
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
        /// Checked property name
        /// </summary>
        public string PropertyName { get; } = propertyName;

        /// <summary>
        /// Checked property values (one of the value is required in order to match this condition; if not given, the value is required if the checked property has a non-
        /// <see langword="null"/> value)
        /// </summary>
        public object?[] Values { get; } = values;

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
            if (PropertyGetter is null)
            {
                Property = validationContext.ObjectInstance.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                    ?? throw new InvalidDataException($"Property {PropertyName} not found in validated type {validationContext.ObjectInstance.GetType()}");
                PropertyGetter = Property.GetGetterDelegate()
                    ?? throw new InvalidDataException($"Property {validationContext.ObjectInstance.GetType()}.{Property.Name} requires a public getter");
            }
            object? v = PropertyGetter(validationContext.ObjectInstance);
            return (Values.Length > 0 && IfNotInValues != Values.Contains(v)) || (Values.Length == 0 && RequiredIfNull == (v is null))
                ? base.IsValid(value, validationContext)
                : null;
        }
    }
}
