using System.Collections;
using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace wan24.ObjectValidation
{
    // Internals
    public static partial class ValidationExtensions
    {
        /// <summary>
        /// Unsigned numeric enum types
        /// </summary>
        private static readonly FrozenSet<Type> UnsignedNumericEnumTypes = new Type[] { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) }.ToFrozenSet();

        /// <summary>
        /// Validate an object
        /// </summary>
        /// <param name="info">Validation information</param>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="throwOnError">Throw a <see cref="ObjectValidationException"/> on any error?</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Valid?</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, which is the default)</exception>
        internal static bool ValidateObject(
            ValidationInfo info,
            object obj,
            List<ValidationResult>? results,
            string? member,
            bool throwOnError,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
        {
            // Update array level and recursion information
            if (info.ArrayLevel != 0)
            {
                info = info.GetClone();
                info.ArrayLevel = 0;
            }
            info.CurrentDepth++;
            int seenIndex = 0;
            Type type = obj.GetType();// Given object type
            string contextInfo = $"(depth {info.CurrentDepth}, array level {info.ArrayLevel}{(string.IsNullOrWhiteSpace(member) ? string.Empty : $", member {member}")})";// Validation stack context information
            try
            {
                // Skip object that disabled the validation or which has a not supported type
                if (!ValidatableTypes.IsTypeValidatable(type)) return true;
                // Avoid an endless recursion
                seenIndex = info.Seen.Count;
                if (!info.Seen.Add(obj)) return true;
                // Prepare the results
                List<ValidationResult> validationResults = [],// Single validation results (will be added to all validation results after a validation)
                    allResults = [];// All validation results (will be added to the given results list, if any)
                bool res = true,// Overall result
                    cancelled,// If an event was cancelled
                    failed = false,// If the object validation failed during event handling
                    isObjectValidatable = obj is IObjectValidatable;// Is an IObjectValidatable object?
                bool Finalize()
                {
                    // Finalize the validation results
                    AddResults(results, allResults, validationResults, member);
                    if (results is not null)
                    {
                        int addErrors = allResults.Count;
                        if (addErrors > 0)
                        {
                            if (MaxErrors > 0) addErrors = MaxErrors - results.Count - addErrors;
                            results.AddRange(allResults.Take(addErrors));
                        }
                    }
                    if (res &= allResults.Count == 0) return true;
                    RaiseEvent(OnObjectValidationFailed, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res);
                    string error = $"Object validation of {type} {contextInfo} failed with {allResults.Count} error(s)";
                    if (throwOnError) throw new ObjectValidationException(allResults, error);
                    ObjectValidation.ValidateObject.Logger(error);
                    return false;
                }
                try
                {
                    // Run event handlers
                    (cancelled, res, failed) = RaiseEvent(OnObjectValidation, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res);
                    if (cancelled)
                    {
#if DEBUG
                        ObjectValidation.ValidateObject.Logger($"Event handler cancelled {type} {contextInfo} validation");
#endif
                        return Finalize();
                    }
                    // Use the default object validation
                    ValidationContext? validationContext = null;
                    if (!isObjectValidatable && !type.IsEnum)
                    {
                        validationContext = new(obj, serviceProvider, items: null);
                        res &= Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);
                        if (obj is IValidatableObject validatable)
                        {
                            validationResults.AddRange(validatable.Validate(validationContext));
                            if (validationResults.Count > 0)
                            {
                                res = false;
                                if (!AddResults(results, allResults, validationResults, member))
                                {
#if DEBUG
                                    ObjectValidation.ValidateObject.Logger($"{type} {contextInfo} validation stopped after default object validation");
#endif
                                    return Finalize();
                                }
                            }
                        }
                    }
                    // Validate an enumeration value
                    if (type.IsEnum)
                    {
                        Type numericType = type.GetEnumUnderlyingType() ?? throw new InvalidProgramException($"Enumeration {type} {contextInfo} without underlying numeric type");
                        if (type.GetCustomAttributesCached().Any(a => a is FlagsAttribute))
                        {
                            bool err;
                            object number,
                                undefinedValue = null!;
                            if (UnsignedNumericEnumTypes.Contains(numericType))
                            {
                                ulong allValues = 0,
                                    numericValue = numericType == typeof(ulong)
                                        ? (ulong)Convert.ChangeType(obj, numericType)
                                        : (ulong)Convert.ChangeType(Convert.ChangeType(obj, numericType), typeof(ulong));
                                number = numericValue;
                                foreach (object v in Enum.GetValues(type)) allValues |= (ulong)Convert.ChangeType(Convert.ChangeType(v, numericType), typeof(ulong));
                                err = (numericValue & ~allValues) != 0;
                                if (err) undefinedValue = numericValue & ~allValues;
                            }
                            else
                            {
                                long allValues = 0,
                                    numericValue = numericType == typeof(long)
                                        ? (long)Convert.ChangeType(obj, numericType)
                                        : (long)Convert.ChangeType(Convert.ChangeType(obj, numericType), typeof(long));
                                number = numericValue;
                                foreach (object v in Enum.GetValues(type)) allValues |= (long)Convert.ChangeType(Convert.ChangeType(v, numericType), typeof(long));
                                err = (numericValue & ~allValues) != 0;
                                if (err) undefinedValue = numericValue & ~allValues;
                            }
                            if (err) validationResults.Add(new($"Undefined enumeration flags value {number} (undefined flag(s) {undefinedValue})"));
                        }
                        else if (!Enum.IsDefined(type, obj))
                        {
                            validationResults.Add(new($"Undefined enumeration value {Convert.ChangeType(obj, numericType)}"));
                        }
                        return Finalize();
                    }
                    // Validate properties
                    object? value;// Property value
                    string memberName;// Full property member name
                    Type valueType;// Property value type
#pragma warning disable IDE0018 // Can be declared inline
                    Type? itemType,// Property item type
                        keyType;// Property dictionary value key type
#pragma warning restore IDE0018 // Can be declared inline
                    ValidationAttribute[] validationAttributes;// Validation attributes
                    ValidationResult[] multiValidationResults;// Multiple validation results
                    bool propFailed = true,// If the property validation failed
                        loopCancelled = false,// If the property validation loop was cancelled
                        noValidation,// Property value validation disabled?
                        noItemValidation = (type.GetCustomAttribute<ItemNoValidationAttribute>(inherit: true)?.ArrayLevel ?? -1)
                            == info.ArrayLevel,// Property value item validation disabled?
                        onlyItemNullValueChecks,// If value items should only be checked for nullability
                        valueValidatable;// If the property value is validatable
                    NullabilityInfoContext nullabilityContext = new();// Context for nullable validation
                    NullabilityInfo nullabilityInfo;// Nullability info
                    NoValidationAttribute? noValidationAttr;// No validation attribute
                    foreach (PropertyInfo pi in members is null 
                        ? type.GetPropertiesCached() 
                        : from pi in type.GetPropertiesCached()
                          where members.Contains(pi.Name)
                          select pi
                        )
                    {
                        // Break the loop, if requested
                        if (loopCancelled)
                        {
#if DEBUG
                            ObjectValidation.ValidateObject.Logger($"{type} {contextInfo} property validation loop cancelled before {pi.Name}");
#endif
                            break;
                        }
                        try
                        {
                            // Run event handlers
                            propFailed = true;
                            (cancelled, res, propFailed) = RaiseEvent(OnObjectPropertyValidation, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res, pi);
                            if (cancelled)
                            {
#if DEBUG
                                ObjectValidation.ValidateObject.Logger($"Event handler cancelled {type}.{(member is null ? pi.Name : $"{member}.{pi.Name}")} {contextInfo} validation");
#endif
                                continue;
                            }
                            // Get the full property member name and its value
                            memberName = member is null ? pi.Name : $"{member}.{pi.Name}";
                            try
                            {
                                value = pi.GetGetterDelegate()!(obj);
                            }
                            catch (Exception ex)
                            {
                                if (pi.SetMethod?.IsPublic ?? !IgnoreGetOnlyErrors) throw;
                                ObjectValidation.ValidateObject.Logger(
                                    $"Skipped property {type}.{pi.Name} (depth {info.CurrentDepth}, array level {info.ArrayLevel}, member {memberName}) value validation: {ex.Message}"
                                    );
#if DEBUG
                                Debugger.Break();
#endif
                                continue;
                            }
                            // Default property validation
                            validationAttributes = pi.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray();
                            noValidationAttr = (NoValidationAttribute?)validationAttributes.FirstOrDefault(a => a is NoValidationAttribute);
                            noValidation = noValidationAttr is not null;
                            if (isObjectValidatable)
                            {
                                if (value is not null)
                                {
                                    validationContext = new(obj, serviceProvider, items: null)
                                    {
                                        MemberName = pi.Name
                                    };
                                    res &= Validator.TryValidateProperty(value, validationContext, validationResults);
                                }
                                if (noValidation && noValidationAttr!.SkipNullValueCheck)
                                {
#if DEBUG
                                    ObjectValidation.ValidateObject.Logger($"Skip {type}.{memberName} {contextInfo} value validation (disabled by {typeof(NoValidationAttribute)})");
#endif
                                    continue;
                                }
                            }
                            else
                            {
                                validationContext = null;
                            }
                            // Multiple validation attributes
                            foreach (IMultipleValidations attr in validationAttributes.Where(a => a is not IItemValidationAttribute && a is IMultipleValidations).Cast<IMultipleValidations>())
                            {
                                validationContext ??= new(obj, serviceProvider, items: null)
                                {
                                    MemberName = pi.Name
                                };
                                multiValidationResults = attr.MultiValidation(value, validationContext, serviceProvider).ToArray();
                                if (multiValidationResults.Length == 0) continue;
                                res = false;
                                validationResults.AddRange(multiValidationResults);
                            }
                            // Ensure a valid value (shouldn't be NULL, if the property type isn't nullable) and skip NULL values
                            nullabilityInfo = nullabilityContext.Create(pi.GetMethod!.ReturnParameter);
                            if (value is null)
                            {
                                if (!IsNullable(pi, nullabilityInfo))
                                {
                                    res = false;
                                    validationResults.Add(new(
                                        $"Property {pi.Name} value is NULL, but the property type {pi.PropertyType} isn't nullable (or a non-NULL value is required)",
                                        [pi.Name]
                                        ));
                                    (_, res, _) = RaiseEvent(OnObjectPropertyValidationFailed, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res);
                                }
                                continue;
                            }
                            // Deep object validation
                            valueType = value.GetType();
                            noValidationAttr = (NoValidationAttribute?)valueType.GetCustomAttributesCached().FirstOrDefault(a => a is NoValidationAttribute);
                            onlyItemNullValueChecks = noValidationAttr is not null && !noValidationAttr.SkipNullValueCheck;
                            if (
                                !(noValidationAttr?.SkipNullValueCheck ?? false) &&
                                valueType.GetCustomAttributesCached().Any(a => a.GetType().FullName == ReflectionHelper.VALIDATENEVER_ATTRIBUTE_TYPE)
                                )
                            {
#if DEBUG
                                ObjectValidation.ValidateObject.Logger($"Skip {type}.{memberName} {contextInfo} value validation (disabled by attribute)");
#endif
                                continue;
                            }
                            valueValidatable = ValidatableTypes.IsTypeValidatable(valueType);
                            if (!noItemValidation || onlyItemNullValueChecks)
                            {
                                if (AsDictionary(value, out keyType, out itemType) is IDictionary dict)
                                {
                                    res &= ValidateDictionary(
                                        info,
                                        pi,
                                        dict,
                                        valueType,
                                        keyType!,
                                        itemType!,
                                        nullabilityInfo,
                                        validationResults,
                                        serviceProvider,
                                        onlyItemNullValueChecks,
                                        throwOnError
                                        );
                                    continue;
                                }
                                else if (value is not string && value is Array arr && valueType.IsArray && valueType.GetElementType() is not null)
                                {
                                    res &= ValidateList(
                                        info,
                                        pi,
                                        arr,
                                        valueType,
                                        valueType.GetElementType(),
                                        nullabilityInfo,
                                        validationResults,
                                        serviceProvider,
                                        onlyItemNullValueChecks,
                                        throwOnError
                                        );
                                    continue;
                                }
                                else if (AsList(value, out itemType) is IList list)
                                {
                                    res &= ValidateList(info, pi, list, valueType, itemType, nullabilityInfo, validationResults, serviceProvider, onlyItemNullValueChecks, throwOnError);
                                    continue;
                                }
                                else if (valueValidatable && value is ICollection col)
                                {
                                    res &= ValidateList(info, pi, col, valueType, itemType, nullabilityInfo, validationResults, serviceProvider, onlyItemNullValueChecks, throwOnError);
                                }
                                else if (valueValidatable && value is IEnumerable enumerable)
                                {
                                    res &= ValidateList(
                                        info,
                                        pi,
                                        enumerable,
                                        valueType,
                                        itemType: null,
                                        nullabilityInfo,
                                        validationResults,
                                        serviceProvider,
                                        onlyItemNullValueChecks,
                                        throwOnError
                                        );
                                }
                            }
#if DEBUG
                            else
                            {
                                ObjectValidation.ValidateObject.Logger(
                                    $"Skip {type}.{memberName} {contextInfo} item validation ({(valueValidatable ? "no item validation" : $"value type {valueType} not validatable")})"
                                    );
                            }
#endif
                            if (valueValidatable)
                            {
                                if (!onlyItemNullValueChecks) res &= ValidateObject(info, value, validationResults, pi.Name, throwOnError, serviceProvider: serviceProvider);
                            }
#if DEBUG
                            else
                            {
                                ObjectValidation.ValidateObject.Logger($"Skip {type}.{memberName} {contextInfo} property value type {valueType} value validation (type isn't validatable)");
                            }
#endif
                        }
                        catch (ObjectValidationException ex)
                        {
                            if (throwOnError) throw;
                            res = false;
                            validationResults.Add(new($"{VALIDATION_EXCEPTION_PREFIX}{pi.Name}: {ex}", [pi.Name]));
                        }
                        catch (Exception ex)
                        {
                            res = false;
                            validationResults.Add(new($"{VALIDATION_EXCEPTION_PREFIX}{pi.Name}: {ex}", [pi.Name]));
                        }
                        finally
                        {
                            if (propFailed || validationResults.Count != 0)
                                (loopCancelled, _, _) = RaiseEvent(OnObjectPropertyValidationFailed, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res, pi);
                            if (!AddResults(results, allResults, validationResults, member)) loopCancelled = true;
                        }
                    }
                    return Finalize();
                }
                catch (Exception ex)
                {
                    validationResults.Add(new($"{VALIDATION_EXCEPTION_PREFIX}{ex}"));
                    return Finalize();
                }
            }
            finally
            {
                info.CurrentDepth--;
                if (seenIndex > 0) info.Seen.Remove(obj);
                if (results is not null)
                    foreach (ValidationResult result in results)
                        ObjectValidation.ValidateObject.Logger(
                            $"Object {type} {contextInfo} validation error: {result.ErrorMessage} (members {(result.MemberNames.Any() ? string.Join(", ", result.MemberNames) : "[none]")})"
                            );
            }
        }

        /// <summary>
        /// Add results to all results and clear the results (prepend the member name, if any)
        /// </summary>
        /// <param name="results">Results</param>
        /// <param name="allResults">All results</param>
        /// <param name="validationResults">Results</param>
        /// <param name="member">Member name</param>
        /// <returns>Continue?</returns>
        internal static bool AddResults(List<ValidationResult>? results, List<ValidationResult> allResults, List<ValidationResult> validationResults, string? member)
        {
            if (validationResults.Count == 0) return MaxErrors < 1 || (results?.Count ?? 0) + allResults.Count < MaxErrors;
            if (member is not null)
                for (int i = 0; i < validationResults.Count; i++)
                    validationResults[i] = new(
                        validationResults[i].ErrorMessage,
                        !validationResults[i].MemberNames.Any()
                            ? [member]
                            : from m in validationResults[i].MemberNames
                              select $"{member}.{m}"
                        );
            allResults.AddRange(validationResults);
            validationResults.Clear();
            return MaxErrors < 1 || (results?.Count ?? 0) + allResults.Count < MaxErrors;
        }

        /// <summary>
        /// Determine if nullable
        /// </summary>
        /// <param name="ni">Nullability info</param>
        /// <returns>Is nullable?</returns>
        internal static bool IsNullable(NullabilityInfo ni) => ni.WriteState != NullabilityState.NotNull || ni.ReadState != NullabilityState.NotNull;

        /// <summary>
        /// Determine if nullable (checks for nullability attributes and info, if given)
        /// </summary>
        /// <param name="obj">Object with custom attributes</param>
        /// <param name="ni">Nullability info</param>
        /// <param name="isItem">Is an item?</param>
        /// <param name="arrayLevel">Array level</param>
        /// <returns>Is nullable?</returns>
        internal static bool IsNullable(ICustomAttributeProvider obj, NullabilityInfo? ni = null, bool isItem = false, int arrayLevel = 0)
        {
            FrozenSet<Attribute> attributes = obj.GetCustomAttributesCached();
            if (!isItem)
            {
                if (attributes.Any(a => a is DisallowNullAttribute)) return false;
                if (attributes.Any(a => a is AllowNullAttribute)) return true;
            }
            if (isItem && attributes.Any(a => a is ItemNullableAttribute attr && attr.ArrayLevel == arrayLevel)) return true;
            if (ni is not null && !IsNullable(ni)) return false;
            return true;
        }

        /// <summary>
        /// Get an object as dictionary (if it is a dictionary)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="keyType">Key type</param>
        /// <param name="valueType">Value type</param>
        /// <returns>Dictionary</returns>
        internal static IDictionary? AsDictionary(object obj, out Type? keyType, out Type? valueType)
        {
            keyType = null;
            valueType = null;
            bool isDict = false;
            for (Type? type = obj.GetType(); type is not null; type = type.BaseType)
            {
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Dictionary<,>)) continue;
                isDict = true;
                Type[] gp = type.GetGenericArguments();
                keyType = gp[0];
                valueType = gp[1];
                break;
            }
            return isDict ? obj as IDictionary : null;
        }

        /// <summary>
        /// Get an object as list (if it is a list)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="itemType">Item type</param>
        /// <returns>List</returns>
        internal static IList? AsList(object obj, out Type? itemType)
        {
            itemType = null;
            bool isList = false;
            for (Type? type = obj.GetType(); type is not null; type = type.BaseType)
            {
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>)) continue;
                isList = true;
                itemType = type.GetGenericArguments()[0];
                break;
            }
            return isList ? obj as IList : null;
        }

        /// <summary>
        /// Determine if a type is abstract
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Is abstract?</returns>
        internal static bool IsAbstractType(Type type) => type.IsAbstract || type.IsInterface || type == typeof(object);
    }
}
