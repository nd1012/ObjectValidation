using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Direct value validation using validation attributes
    /// </summary>
    public static class ValueValidation
    {
        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        public static IEnumerable<ValidationResult> ValidateValue<T>(this T? value, IEnumerable<ValidationAttribute> attributes, IServiceProvider? serviceProvider = null)
        {
            ValidationContext context = new(new(), serviceProvider, items: null);
            foreach (ValidationAttribute attr in attributes)
                if (attr.GetValidationResult(value, context) is ValidationResult res)
                    yield return res;
        }

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        public static IEnumerable<ValidationResult> ValidateValue<T>(this T? value, params ValidationAttribute[] attributes)
            => ValidateValue(value, attributes, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        public static IEnumerable<ValidationResult> ValidateValue<T>(this T? value, IServiceProvider serviceProvider, params ValidationAttribute[] attributes)
            => ValidateValue(value, attributes, serviceProvider);

        /// <summary>
        /// Validate a value directly using a validation template
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.PropertyValidations"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Tiny method")]
        public static IEnumerable<ValidationResult> ValidateValue<T>(this T? value, string template, IServiceProvider? serviceProvider = null)
            => ValidationTemplates.PropertyValidations.TryGetValue(template, out List<ValidationAttribute>? attributes)
                ? ValidateValue(value, attributes, serviceProvider)
                : throw new ArgumentException("Validation template not found", nameof(template));

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        public static IEnumerable<ValidationResult> ValidateItem<T>(this T? value, IEnumerable<IItemValidationAttribute> attributes, IServiceProvider? serviceProvider = null)
        {
            ValidationContext context = new(new(), serviceProvider, items: null);
            foreach (IItemValidationAttribute attr in attributes)
                if (attr.GetValidationResult(value, context, serviceProvider) is ValidationResult res)
                    yield return res;
        }

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        public static IEnumerable<ValidationResult> ValidateItem<T>(this T? value, params IItemValidationAttribute[] attributes)
            => ValidateItem(value, attributes, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        public static IEnumerable<ValidationResult> ValidateItem<T>(this T? value, IServiceProvider serviceProvider, params IItemValidationAttribute[] attributes)
            => ValidateItem(value, attributes, serviceProvider);

        /// <summary>
        /// Validate a value directly using a validation template
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.ItemValidations"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        [TargetedPatchingOptOut("Tiny method")]
        public static IEnumerable<ValidationResult> ValidateItem<T>(this T? value, string template, IServiceProvider? serviceProvider = null)
            => ValidationTemplates.ItemValidations.TryGetValue(template, out List<IItemValidationAttribute>? attributes)
                ? ValidateItem(value, attributes, serviceProvider)
                : throw new ArgumentException("Validation template not found", nameof(template));

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, IEnumerable<ValidationAttribute> attributes, IServiceProvider? serviceProvider = null)
            => TryValidate(value, attributes, out IEnumerable<ValidationResult> results, serviceProvider)
                ? throw new ObjectValidationException(results)
                : value;

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, params ValidationAttribute[] attributes)
            => Validate(value, attributes, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, IServiceProvider serviceProvider, params ValidationAttribute[] attributes)
            => Validate(value, attributes, serviceProvider);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.PropertyValidations"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? ValidateAsValue<T>(this T? value, string template, IServiceProvider? serviceProvider = null)
            => TryValidateAsValue(value, template, out IEnumerable<ValidationResult> results, serviceProvider)
                ? throw new ObjectValidationException(results)
                : value;

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, IEnumerable<IItemValidationAttribute> attributes, IServiceProvider? serviceProvider = null)
            => TryValidate(value, attributes, out IEnumerable<ValidationResult> results, serviceProvider)
                ? throw new ObjectValidationException(results)
                : value;

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, params IItemValidationAttribute[] attributes)
            => Validate(value, attributes, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? Validate<T>(this T? value, IServiceProvider serviceProvider, params IItemValidationAttribute[] attributes)
            => Validate(value, attributes, serviceProvider);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.PropertyValidations"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Validation results</returns>
        /// <exception cref="ObjectValidationException">Validation failed</exception>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static T? ValidateAsItem<T>(this T? value, string template, IServiceProvider? serviceProvider = null)
            => TryValidateAsItem(value, template, out IEnumerable<ValidationResult> results, serviceProvider)
                ? throw new ObjectValidationException(results)
                : value;

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="results">Validation results</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(
            this T? value,
            IEnumerable<ValidationAttribute> attributes,
            out IEnumerable<ValidationResult> results,
            IServiceProvider? serviceProvider = null
            )
            => !(results = ValidateValue(value, attributes, serviceProvider)).Any();

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="results">Validation results</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(this T? value, out IEnumerable<ValidationResult> results, params ValidationAttribute[] attributes)
            => TryValidate(value, attributes, out results, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="results">Validation results</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(
            this T? value,
            IServiceProvider serviceProvider,
            out IEnumerable<ValidationResult> results,
            params ValidationAttribute[] attributes
            )
            => TryValidate(value, attributes, out results, serviceProvider);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.PropertyValidations"/>)</param>
        /// <param name="results">Validation results</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidateAsValue<T>(
            this T? value,
            string template,
            out IEnumerable<ValidationResult> results,
            IServiceProvider? serviceProvider = null
            )
            => !(results = ValidateValue(value, template, serviceProvider)).Any();

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="attributes">Validation attributes</param>
        /// <param name="results">Validation results</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(
            this T? value,
            IEnumerable<IItemValidationAttribute> attributes,
            out IEnumerable<ValidationResult> results,
            IServiceProvider? serviceProvider = null
            )
            => !(results = ValidateItem(value, attributes, serviceProvider)).Any();

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="results">Validation results</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(this T? value, out IEnumerable<ValidationResult> results, params IItemValidationAttribute[] attributes)
            => TryValidate(value, attributes, out results, serviceProvider: null);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="results">Validation results</param>
        /// <param name="attributes">Validation attributes</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Just a method adapter")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidate<T>(
            this T? value,
            IServiceProvider serviceProvider,
            out IEnumerable<ValidationResult> results,
            params IItemValidationAttribute[] attributes
            )
            => TryValidate(value, attributes, out results, serviceProvider);

        /// <summary>
        /// Validate a value directly
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="template">Validation template name (<see cref="ValidationTemplates.PropertyValidations"/>)</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="results">Validation results</param>
        /// <returns>If the value was validated without any error</returns>
        [TargetedPatchingOptOut("Tiny method")]
        [return: NotNullIfNotNull(nameof(value))]
        public static bool TryValidateAsItem<T>(
            this T? value,
            string template,
            out IEnumerable<ValidationResult> results,
            IServiceProvider? serviceProvider = null
            )
            => !(results = ValidateValue(value, template, serviceProvider)).Any();
    }
}
