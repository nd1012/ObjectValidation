using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item property compare attribute
    /// </summary>
    public class ItemCompareAttribute : ItemValidationAttribute
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
        /// Constructor
        /// </summary>
        /// <param name="ownProperty">Own property name</param>
        /// <param name="otherProperty">Other property name</param>
        /// <param name="target">Validation target</param>
        public ItemCompareAttribute(string ownProperty, string otherProperty, ItemValidationTargets target = ItemValidationTargets.Item)
            : base(target, new CompareAttribute(otherProperty))
            => OwnProperty = ownProperty;

        /// <summary>
        /// Own property name
        /// </summary>
        public string OwnProperty { get; }

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
