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
        /// <param name="onlyNullCheck">Only <see langword="null"/> checks?</param>
        /// <param name="throwOnError">Throw an exception on error?</param>
        /// <param name="member">Member name</param>
        /// <returns>Overall result</returns>
        internal static bool ValidateDictionary(
            ValidationInfo info,
            PropertyInfo pi,
            IDictionary dict,
            Type valueType,
            Type? keyType,
            Type? itemType,
            NullabilityInfo? nullabilityInfo,
            List<ValidationResult> validationResults,
            IServiceProvider? serviceProvider,
            bool onlyNullCheck,
            bool throwOnError,
            string? member = null
            )
        {
            if (keyType != null && IsAbstractType(keyType)) keyType = null;
            if (itemType != null && IsAbstractType(itemType)) itemType = null;
            bool res = true,// Overall result
                keyValidatable = keyType == null || ValidatableTypes.IsTypeValidatable(keyType),// If the key is validatable
                itemValidatable = itemType == null || ValidatableTypes.IsTypeValidatable(itemType);// If an item is validatable
            IItemValidationAttribute[] keyValidations = onlyNullCheck
                ? Array.Empty<IItemValidationAttribute>()
                : GetItemValidations(pi, info.ArrayLevel, ItemValidationTargets.Key);// Key validations
            if (keyValidations.Length != 0 && IsNoItemValidation(keyValidations)) keyValidations = Array.Empty<IItemValidationAttribute>();
            IItemValidationAttribute[] valueValidations = onlyNullCheck
                ? Array.Empty<IItemValidationAttribute>()
                : GetItemValidations(pi, info.ArrayLevel);// Value validations
            if (valueValidations.Length != 0 && IsNoItemValidation(valueValidations, ref onlyNullCheck)) valueValidations = Array.Empty<IItemValidationAttribute>();
#if DEBUG
            if (!onlyNullCheck)
            {
                if (!keyValidatable) ObjectValidation.ValidateObject.Logger($"{valueType} ({pi.DeclaringType}.{pi.Name}, member \"{member}\") keys are not validatable");
                if (!itemValidatable) ObjectValidation.ValidateObject.Logger($"{valueType} ({pi.DeclaringType}.{pi.Name}, member \"{member}\") items are not validatable");
            }
#endif
            bool valueNullable = (nullabilityInfo != null && pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>) && IsNullable(nullabilityInfo.GenericTypeArguments[1])) ||
                pi.GetCustomAttribute<ItemNullableAttribute>(inherit: true) != null;// If values are nullable
            if (valueNullable && onlyNullCheck) return res;
            int count = 0;// Key/value pair count
            object? val;// Value
            foreach (object key in dict.Keys)
            {
                count++;
                // Key validations
                if (keyValidations.Length != 0)
                    res &= ValidateItem(info, pi, GetMemberName(info, pi, count, member, ItemValidationTargets.Key, isDict: true), key, keyValidations, serviceProvider, validationResults, throwOnError);
                if (keyValidatable && (keyType != null || ValidatableTypes.IsTypeValidatable(key.GetType())))
                {
                    res &= ValidateObject(info, key, validationResults, GetMemberName(info, pi, count, member, ItemValidationTargets.Key, isDict: true), throwOnError);
                }
#if DEBUG
                else if (keyType == null)
                {
                    ObjectValidation.ValidateObject.Logger($"{valueType} member {GetMemberName(info, pi, count, member)} key type {key.GetType()} is not validatable");
                }
#endif
                // Value validations
                val = dict[key];
                if (val == null)
                {
                    if (!valueNullable)
                    {
                        res = false;
                        validationResults.Add(new(
                            $"Property {GetMemberName(info, pi, count, member, isDict: true)} value is NULL, but the value type {itemType} isn't nullable (a non-NULL value is required)",
                            new string[] { GetMemberName(info, pi, count, member, isDict: true) }
                            ));
                    }
                    else if (valueValidations.Length != 0)
                    {
                        res &= ValidateItem(info, pi, GetMemberName(info, pi, count, member, isDict: true), val, valueValidations, serviceProvider, validationResults, throwOnError);
                    }
                    continue;
                }
                if (onlyNullCheck) continue;
                if (valueValidations.Length != 0)
                    res &= ValidateItem(info, pi, GetMemberName(info, pi, count, member, isDict: true), val, valueValidations, serviceProvider, validationResults, throwOnError);
                if (itemValidatable && (itemType != null || ValidatableTypes.IsTypeValidatable(val.GetType())))
                {
                    res &= ValidateObject(info, val, validationResults, GetMemberName(info, pi, count, member, isDict: true), throwOnError, serviceProvider: serviceProvider);
                }
#if DEBUG
                else if (itemType == null)
                {
                    ObjectValidation.ValidateObject.Logger($"{valueType} member {GetMemberName(info, pi, count, member)} item type {val.GetType()} is not validatable");
                }
#endif
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
        /// <param name="onlyNullCheck">Only <see langword="null"/> checks?</param>
        /// <param name="throwOnError">Throw an exception on error?</param>
        /// <param name="member">Member name</param>
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
            bool onlyNullCheck,
            bool throwOnError,
            string? member = null
            )
        {
            if (itemType != null && IsAbstractType(itemType)) itemType = null;
            IItemValidationAttribute[] itemValidations = onlyNullCheck
                ? Array.Empty<IItemValidationAttribute>()
                : GetItemValidations(pi, info.ArrayLevel);// Item validations
            bool res = true,// Overall result
                itemValidatable = itemType == null || ValidatableTypes.IsTypeValidatable(itemType);// If an item is validatable
            if (itemValidations.Length != 0 && IsNoItemValidation(itemValidations, ref onlyNullCheck)) itemValidations = Array.Empty<IItemValidationAttribute>();
            if (!itemValidatable && !onlyNullCheck)
            {
#if DEBUG
                ObjectValidation.ValidateObject.Logger($"{valueType} ({pi.DeclaringType}.{pi.Name}, member \"{member}\") items are not validatable");
#endif
                if (itemValidations.Length == 0) return res;
            }
            bool itemNullable = (nullabilityInfo != null && pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && IsNullable(nullabilityInfo.GenericTypeArguments[0])) ||
                (valueType.IsArray && nullabilityInfo?.ElementType != null && IsNullable(nullabilityInfo.ElementType)) ||
                pi.GetCustomAttribute<ItemNullableAttribute>(inherit: true) != null;// If items are nullable
            if (itemNullable && onlyNullCheck) return res;
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
                            $"Property {GetMemberName(info, pi, count, member)} value is NULL, but the item type {itemType} isn't nullable (a non-NULL value is required)",
                            new string[] { GetMemberName(info, pi, count, member) }
                            ));
                    }
                    else if (itemValidations.Length != 0)
                    {
                        res &= ValidateItem(info, pi, GetMemberName(info, pi, count, member), val, itemValidations, serviceProvider, validationResults, throwOnError);
                    }
                    continue;
                }
                if (onlyNullCheck) continue;
                if (itemValidations.Length != 0)
                    res &= ValidateItem(info, pi, GetMemberName(info, pi, count, member), val, itemValidations, serviceProvider, validationResults, throwOnError);
                if (itemValidatable && (itemType != null || ValidatableTypes.IsTypeValidatable(val.GetType())))
                {
                    res &= ValidateObject(info, val, validationResults, GetMemberName(info, pi, count, member), throwOnError, serviceProvider: serviceProvider);
                }
#if DEBUG
                else if (itemType == null)
                {
                    ObjectValidation.ValidateObject.Logger($"{valueType} member {GetMemberName(info, pi, count, member)} item type {val.GetType()} is not validatable");
                }
#endif
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
            ValidationResult[] multiResults;// Multiplevalidation results
            bool res = true;// Overall result
            // Default validations
            foreach (IItemValidationAttribute attr in attrs)
                if (attr is IMultipleValidations multiValidation)
                {
                    multiResults = multiValidation.MultiValidation(value, context, serviceProvider).ToArray();
                    if (multiResults.Length == 0) continue;
                    res = false;
                    validationResults.AddRange(multiResults);
                }
                else
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
                bool valueValidatable = ValidatableTypes.IsTypeValidatable(valueType);// If the value type is validatable
#pragma warning disable IDE0018 // Declare inline
                Type? keyType,// Dictionary key type
                    itemType;// Item type
#pragma warning restore IDE0018 // Declare inline
                int seenIndex;// Seen index
                if (AsDictionary(value, out keyType, out itemType) is IDictionary dict)
                {
                    seenIndex = info.Seen.Count;
                    info.Seen.Add(dict);
                    try
                    {
                        nestedInfo.CurrentDepth++;
                        res &= ValidateDictionary(
                            nestedInfo,
                            pi,
                            dict,
                            valueType,
                            keyType!,
                            itemType!,
                            nullabilityInfo: null,
                            validationResults,
                            serviceProvider,
                            onlyNullCheck: false,
                            throwOnError,
                            member
                            );
                    }
                    finally
                    {
                        info.Seen.RemoveAt(seenIndex);
                    }
                }
                else if (value is not string && value is Array arr && valueType.IsArray && valueType.GetElementType() != null)
                {
                    seenIndex = info.Seen.Count;
                    info.Seen.Add(arr);
                    try
                    {
                        nestedInfo.CurrentDepth++;
                        res &= ValidateList(
                            nestedInfo,
                            pi,
                            arr,
                            valueType,
                            valueType.GetElementType(),
                            nullabilityInfo: null,
                            validationResults,
                            serviceProvider,
                            onlyNullCheck: false,
                            throwOnError,
                            member
                            );
                    }
                    finally
                    {
                        info.Seen.RemoveAt(seenIndex);
                    }
                }
                else if (AsList(value, out itemType) is IList list)
                {
                    seenIndex = info.Seen.Count;
                    info.Seen.Add(list);
                    try
                    {
                        nestedInfo.CurrentDepth++;
                        res &= ValidateList(nestedInfo, pi, list, valueType, itemType, nullabilityInfo: null, validationResults, serviceProvider, onlyNullCheck: false, throwOnError, member);
                    }
                    finally
                    {
                        info.Seen.RemoveAt(seenIndex);
                    }
                }
                else if (valueValidatable && value is ICollection col)
                {
                    seenIndex = info.Seen.Count;
                    info.Seen.Add(col);
                    try
                    {
                        nestedInfo.CurrentDepth++;
                        res &= ValidateList(nestedInfo, pi, col, valueType, itemType, nullabilityInfo: null, validationResults, serviceProvider, onlyNullCheck: false, throwOnError, member);
                    }
                    finally
                    {
                        info.Seen.RemoveAt(seenIndex);
                    }
                }
                else if (valueValidatable && value is IEnumerable enumerable)
                {
                    seenIndex = info.Seen.Count;
                    info.Seen.Add(enumerable);
                    try
                    {
                        nestedInfo.CurrentDepth++;
                        res &= ValidateList(
                            nestedInfo,
                            pi,
                            enumerable,
                            valueType,
                            itemType: null,
                            nullabilityInfo: null,
                            validationResults,
                            serviceProvider,
                            onlyNullCheck: false,
                            throwOnError,
                            member
                            );
                    }
                    finally
                    {
                        info.Seen.RemoveAt(seenIndex);
                    }
                }
#if DEBUG
                else
                {
                    ObjectValidation.ValidateObject.Logger(
                        $"Can't validate item {member} type {valueType} of property {pi.DeclaringType}.{pi.Name} value (not validatable {pi.PropertyType} value)"
                        );
                }
#endif
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

        /// <summary>
        /// Determine if an item validation was disabled
        /// </summary>
        /// <param name="attrs">Attributes</param>
        /// <param name="onlyNullCheck">Only item value <see langword="null"/> check?</param>
        /// <returns>Item validation is disabled?</returns>
        internal static bool IsNoItemValidation(IItemValidationAttribute[] attrs, ref bool onlyNullCheck)
        {
            if (attrs.FirstOrDefault(a => a is ItemNoValidationAttribute) is not ItemNoValidationAttribute attr) return false;
            if (!onlyNullCheck && !((NoValidationAttribute)attr.ValidationAttribute).SkipNullValueCheck) onlyNullCheck = true;
            return true;
        }

        /// <summary>
        /// Get a member name
        /// </summary>
        /// <param name="info">Validation information</param>
        /// <param name="pi">Property info</param>
        /// <param name="item">Item number</param>
        /// <param name="member">Member name</param>
        /// <param name="target">Validation target</param>
        /// <param name="isDict">Is a dictionary?</param>
        /// <returns>Member name</returns>
        internal static string GetMemberName(ValidationInfo info, PropertyInfo pi, long item, string? member, ItemValidationTargets target = ItemValidationTargets.Item, bool isDict = false)
            => info.ArrayLevel switch
            {
                0 => isDict
                        ? $"{pi.Name}[{(target == ItemValidationTargets.Item ? "value" : "key")}#{item}]"
                        : $"{pi.Name}[{item}]",
                _ => isDict
                        ? $"{member ?? throw new ArgumentNullException(nameof(member))}[{(target == ItemValidationTargets.Item ? "value" : "key")}#{item}]"
                        : $"{member ?? throw new ArgumentNullException(nameof(member))}[{item}]"
            };
    }
}
