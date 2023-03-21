using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ItemValidationAttribute : Attribute, IItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attr">Served validation attribute</param>
        public ItemValidationAttribute(ValidationAttribute attr) : base() => ValidationAttribute = attr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        /// <param name="attr">Served validation attribute</param>
        public ItemValidationAttribute(ItemValidationTargets target, ValidationAttribute attr) : this(attr) => ValidationTarget = target;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Served validation attribute type</param>
        /// <param name="param">Validation</param>
        public ItemValidationAttribute(Type type, params object[] param) : base()
            => ValidationAttribute = Activator.CreateInstance(type, param) as ValidationAttribute ?? throw new ArgumentException("Invalid type or constructor parameters", nameof(type));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        /// <param name="type">Served validation attribute type</param>
        /// <param name="param">Validation</param>
        public ItemValidationAttribute(ItemValidationTargets target, Type type, params object[] param) : this(type, param) => ValidationTarget = target;

        /// <inheritdoc/>
        public ItemValidationTargets ValidationTarget { get; } = ItemValidationTargets.Item;

        /// <summary>
        /// Served validation attribute
        /// </summary>
        public ValidationAttribute ValidationAttribute { get; }

        /// <summary>
        /// Target array level
        /// </summary>
        public int ArrayLevel { get; set; }

        /// <inheritdoc/>
        public virtual ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider)
            => ValidationAttribute.GetValidationResult(value, validationContext);
    }
}
