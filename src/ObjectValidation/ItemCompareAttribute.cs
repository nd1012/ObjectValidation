using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item property compare attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="ownProperty">Own property name</param>
    /// <param name="otherProperty">Other property name</param>
    /// <param name="target">Validation target</param>
    public class ItemCompareAttribute(string ownProperty, string otherProperty, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new CompareAttribute(otherProperty))
    {
        /// <summary>
        /// Own property
        /// </summary>
        protected PropertyInfo? _OwnProperty = null;
        /// <summary>
        /// Own property getter
        /// </summary>
        protected ReflectionHelper.PropertyGetter_Delegate? OwnPropertyGetter = null;

        /// <summary>
        /// Own property name
        /// </summary>
        public string OwnProperty { get; } = ownProperty;

        /// <inheritdoc/>
        public override ValidationResult? GetValidationResult(object? value, ValidationContext validationContext, IServiceProvider? serviceProvider)
        {
            if (value is null) return null;
            if (OwnPropertyGetter is null)
            {
                _OwnProperty = value.GetType().GetProperty(OwnProperty, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? throw new ValidationException($"Property {value.GetType()}.{OwnProperty} not found");
                OwnPropertyGetter = _OwnProperty.GetGetterDelegate()
                    ?? throw new ValidationException($"Property {value.GetType()}.{OwnProperty} has no getter");
            }
            try
            {
                return base.GetValidationResult(OwnPropertyGetter!(value), new(value, serviceProvider, items: null) { MemberName = OwnProperty }, serviceProvider);
            }
            catch(Exception ex)
            {
                throw new ValidationException(
                    $"Exception while comparing {value.GetType()}.{OwnProperty} with {((CompareAttribute)ValidationAttribute).OtherProperty}",
                    ex
                    );
            }
        }
    }
}
