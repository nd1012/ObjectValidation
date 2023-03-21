using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    // Items
    public static partial class ValidationExtensions
    {
        /// <summary>
        /// Validate a dictionary
        /// </summary>
        /// <param name="info">Validation information</param>
        /// <param name="pi">Property info</param>
        /// <param name="dict">Dictionary</param>
        /// <param name="valueType">Value type</param>
        /// <param name="keyType">Key type</param>
        /// <param name="itemType">Item type</param>
        /// <param name="nullabilityInfo">Nullability info</param>
        /// <param name="validationResults">Validation results</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="throwOnError">Throw an exception on error?</param>
        /// <returns>Overall result</returns>
        internal static bool ValidateDictionary(
            ValidationInfo info,
            PropertyInfo pi,
            IDictionary dict,
            Type valueType,
            Type keyType,
            Type itemType,
            NullabilityInfo? nullabilityInfo,
            List<ValidationResult> validationResults,
            IServiceProvider? serviceProvider,
            bool throwOnError
            )
        {
            bool res = true;// Overall result
            IItemValidationAttribute[]? keyValidations = GetItemValidations(pi, info.ArrayLevel, ItemValidationTargets.Key);// Key validations
            if (IsNoItemValidation(keyValidations)) keyValidations = null;
            IItemValidationAttribute[]? valueValidations = GetItemValidations(pi, info.ArrayLevel);// Value validations
            if (IsNoItemValidation(valueValidations)) valueValidations = null;
            if (keyValidations == null && valueValidations == null) return res;
            bool valueNullable = (nullabilityInfo != null && pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>) && IsNullable(nullabilityInfo.GenericTypeArguments[1])) ||
                pi.GetCustomAttribute<ItemNullableAttribute>(inherit: true) != null;// If values are nullable
            int count = 0;// Key/value pair count
            object? val;// Value
            foreach (object key in dict.Keys)
            {
                count++;
                // Key validations
                if (keyValidations != null)
                    res &= ValidateItem(info, pi, $"{pi.Name}[key#{count}]", key, keyValidations, serviceProvider, validationResults, throwOnError);
                if (!keyType!.IsValueType)
                    res &= ValidateObject(info, key, validationResults, $"{pi.Name}[key#{count}]", throwOnError);
                // Value validations
                val = dict[key];
                if (val == null)
                {
                    if (!valueNullable)
                    {
                        res = false;
                        validationResults.Add(new(
                            $"Property {pi.Name}[value#{count}] value is NULL, but the value type {itemType} isn't nullable (a non-null value is required)",
                            new string[] { $"{pi.Name}[value#{count}]" }
                            ));
                    }
                    else if (valueValidations != null)
                    {
                        res &= ValidateItem(info, pi, $"{pi.Name}[value#{count}]", val, valueValidations, serviceProvider, validationResults, throwOnError);
                    }
                    continue;
                }
                if (valueValidations != null)
                    res &= ValidateItem(info, pi, $"{pi.Name}[value#{count}]", val, valueValidations, serviceProvider, validationResults, throwOnError);
                if (!itemType!.IsValueType)
                    res &= ValidateObject(info, val, validationResults, $"{pi.Name}[value#{count}]", throwOnError, serviceProvider: serviceProvider);
            }
            return res;
        }

        /// <summary>
        /// Validate a list
        /// </summary>
        /// <param name="info">Validation information</param>
        /// <param name="pi">Property info</param>
        /// <param name="list">Dictionary</param>
        /// <param name="valueType">Value type</param>
        /// <param name="itemType">Item type</param>
        /// <param name="nullabilityInfo">Nullability info</param>
        /// <param name="validationResults">Validation results</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="throwOnError">Throw an exception on error?</param>
        /// <returns>Overall result</returns>
        internal static bool ValidateList(
            ValidationInfo info,
            PropertyInfo pi,
            IEnumerable list,
            Type valueType,
            Type? itemType,
            NullabilityInfo? nullabilityInfo,
            List<ValidationResult> validationResults,
            IServiceProvider? serviceProvider,
            bool throwOnError
            )
        {
            bool res = true;// Overall result
            IItemValidationAttribute[]? itemValidations = GetItemValidations(pi, info.ArrayLevel);// Item validations
            if (IsNoItemValidation(itemValidations)) return res;
            bool itemNullable = (nullabilityInfo != null && pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && IsNullable(nullabilityInfo.GenericTypeArguments[0])) ||
                (valueType.IsArray && nullabilityInfo?.ElementType != null && IsNullable(nullabilityInfo.ElementType)) ||
                pi.GetCustomAttribute<ItemNullableAttribute>(inherit: true) != null;// If items are nullable
            int count = 0;// Item count
            foreach (object? val in list)
            {
                count++;
                if (val == null)
                {
                    if (!itemNullable)
                    {
                        res = false;
                        validationResults.Add(new(
                            $"Property {pi.Name}[{count}] value is NULL, but the item type {itemType} isn't nullable (a non-null value is required)",
                            new string[] { $"{pi.Name}[{count}]" }
                            ));
                    }
                    else
                    {
                        res &= ValidateItem(info, pi, $"{pi.Name}[{count}]", val, itemValidations, serviceProvider, validationResults, throwOnError);
                    }
                    continue;
                }
                res &= ValidateItem(info, pi, $"{pi.Name}[{count}]", val, itemValidations, serviceProvider, validationResults, throwOnError);
                if (!(itemType ?? val.GetType()).IsValueType)
                    res &= ValidateObject(info, val, validationResults, $"{pi.Name}[{count}]", throwOnError, serviceProvider: serviceProvider);
            }
            return res;
        }

        /// <summary>
        /// Validate an item
        /// </summary>
        /// <param name="info">Validation info</param>
        /// <param name="pi">Property info</param>
        /// <param name="member">Member name</param>
        /// <param name="value">Value</param>
        /// <param name="attrs">Attributes</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <param name="validationResults">Results</param>
        /// <param name="throwOnError">Throw an exception on error?</param>
        /// <returns>Valid?</returns>
        internal static bool ValidateItem(
            ValidationInfo info, 
            PropertyInfo pi, 
            string member, 
            object? value, 
            IItemValidationAttribute[] attrs, 
            IServiceProvider? serviceProvider, 
            List<ValidationResult> validationResults,
            bool throwOnError
            )
        {
            ValidationContext context = new(pi, serviceProvider, items: null) { MemberName = member };// Validation context
            ValidationResult? result;// Validation result
            bool res = true;// Overall result
            // Default validations
            foreach (IItemValidationAttribute attr in attrs)
            {
                if ((result = attr.GetValidationResult(value, context, serviceProvider)) == null) continue;
                res = false;
                validationResults.Add(result);
            }
            // Deep array validation
            if (value != null && (pi.GetCustomAttribute<ItemNoValidationAttribute>(inherit: true)?.ArrayLevel ?? -1) != info.ArrayLevel + 1)
            {
                ValidationInfo nestedInfo = info.GetClone();
                nestedInfo.ArrayLevel++;
                Type valueType = value.GetType();// Value type
                Type? keyType,// Dictionary key type
                    itemType;// Item type
                if (AsDictionary(value, out keyType, out itemType) is IDictionary dict)
                {
                    nestedInfo.CurrentDepth++;
                    res &= ValidateDictionary(nestedInfo, pi, dict, valueType, keyType!, itemType!, nullabilityInfo: null, validationResults, serviceProvider, throwOnError);
                }
                else if (value is Array arr && valueType.IsArray && valueType.GetElementType() != null)
                {
                    nestedInfo.CurrentDepth++;
                    res &= ValidateList(nestedInfo, pi, arr, valueType, valueType.GetElementType(), nullabilityInfo: null, validationResults, serviceProvider, throwOnError);
                }
                else if (AsList(value, out itemType) is IList list)
                {
                    nestedInfo.CurrentDepth++;
                    res &= ValidateList(nestedInfo, pi, list, valueType, itemType, nullabilityInfo: null, validationResults, serviceProvider, throwOnError);
                }
                else if (value is ICollection col)
                {
                    nestedInfo.CurrentDepth++;
                    res &= ValidateList(nestedInfo, pi, col, valueType, itemType, nullabilityInfo: null, validationResults, serviceProvider, throwOnError);
                }
                else if (value is IEnumerable enumerable)
                {
                    nestedInfo.CurrentDepth++;
                    res &= ValidateList(nestedInfo, pi, enumerable, valueType, itemType: null, nullabilityInfo: null, validationResults, serviceProvider, throwOnError);
                }
            }
            return res;
        }

        /// <summary>
        /// Get item validations
        /// </summary>
        /// <param name="pi">Property info</param>
        /// <param name="arrayLevel">Array level</param>
        /// <param name="target">Validation target</param>
        /// <returns>Validations</returns>
        internal static IItemValidationAttribute[] GetItemValidations(PropertyInfo pi, int arrayLevel, ItemValidationTargets target = ItemValidationTargets.Item)
            => (from a in pi.GetCustomAttributes(inherit: true)
                where a is IItemValidationAttribute iva &&
                    iva.ValidationTarget == target && 
                    iva.ArrayLevel == arrayLevel
                select a as IItemValidationAttribute)
                .ToArray();

        /// <summary>
        /// Determine if an item validation was disabled
        /// </summary>
        /// <param name="attrs">Attributes</param>
        /// <returns>Item validation is disabled?</returns>
        internal static bool IsNoItemValidation(IItemValidationAttribute[] attrs) => attrs.Any(a => a is ItemNoValidationAttribute);
    }
}
