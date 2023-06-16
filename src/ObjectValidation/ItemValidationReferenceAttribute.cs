using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute to reference another property for inheriting its validation attributes during an item validation is being executed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ItemValidationReferenceAttribute : Attribute, IItemValidationAttribute
    {
        /// <summary>
        /// Item validation attributes
        /// </summary>
        protected readonly IItemValidationAttribute[] Attributes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Referenced type</param>
        /// <param name="propertyName">Referenced property name</param>
        /// <param name="arrayLevel">Array kevel</param>
        /// <param name="target">Validation target</param>
        public ItemValidationReferenceAttribute(Type type, string propertyName, int arrayLevel = 0, ItemValidationTargets target = ItemValidationTargets.Item) : base()
        {
            ValidationTarget = target;
            ArrayLevel = arrayLevel;
            ReferencedProperty = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new ArgumentException($"Public instance property \"{type}.{propertyName}\" not found", nameof(propertyName));
            Attributes = (from attr in ReferencedProperty.GetItemValidationAttributes()
                          where attr.ArrayLevel == arrayLevel
                          select attr).ToArray();
        }

        /// <inheritdoc/>
        public ItemValidationTargets ValidationTarget { get; }

        /// <inheritdoc/>
        public int ArrayLevel { get; }

        /// <summary>
        /// Referenced property
        /// </summary>
        public PropertyInfo ReferencedProperty { get; }

        /// <inheritdoc/>
        public virtual ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider)
            => MultiValidation(value, validationContext, serviceProvider).FirstOrDefault();

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> MultiValidation(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider = null)
        {
            foreach (IItemValidationAttribute attr in Attributes)
                if (attr.GetValidationResult(value, validationContext, serviceProvider) is ValidationResult result)
                    yield return result;
        }
    }
}
