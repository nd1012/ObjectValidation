using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace wan24.ObjectValidation
{
    // Internals
    public static partial class ValidationExtensions
    {
        /// <summary>
        /// Never validate attribute full type (ASP.NET)
        /// </summary>
        private const string VALIDATENEVER_ATTRIBUTE_TYPE = "Microsoft.AspNetCore.Mvc.ModelBinding.Validation.NeverValidateAttribute";

        /// <summary>
        /// Unsigned numeric enum types
        /// </summary>
        private static readonly Type[] UnsignedNumericEnumTypes = new Type[] { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) };

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
            IServiceProvider? serviceProvider = null)
        {
            // Update array level and recursion informations
            if (info.ArrayLevel != 0)
            {
                info = info.GetClone();
                info.ArrayLevel = 0;
            }
            info.CurrentDepth++;
            int seenIndex = 0;
            try
            {
                // Skip object that disabled the validation or which has a not supported type
                Type type = obj.GetType();// Given object type
                if ((type.IsValueType && !type.IsEnum) || type.IsArray || type == typeof(string) || type == typeof(object) || type.GetCustomAttribute<NoValidationAttribute>(inherit: true) != null)
                    return true;
                // Avoid an endless recursion
                if (info.Seen.Contains(obj)) return true;
                seenIndex = info.Seen.Count;
                info.Seen.Add(obj);
                // Prepare the results
                List<ValidationResult> validationResults = new(),// Single validation results (will be added to all validation results after a validation)
                    allResults = new();// All validation results (will be added to the given results list, if any)
                bool res = true,// Overall result
                    cancelled,// If an event was cancelled
                    failed = false,// If the object validation failed during event handling
                    isObjectValidatable = obj is IObjectValidatable;// Is an IObjectValidatable object?
                bool Finalize()
                {
                    // Finalize the validation results
                    AddResults(results, allResults, validationResults, member);
                    if (results != null)
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
                    if (throwOnError) throw new ObjectValidationException(allResults, "Object validation failed");
                    return false;
                }
                try
                {
                    // Run event handlers
                    (cancelled, res, failed) = RaiseEvent(OnObjectValidation, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res);
                    if (cancelled) return Finalize();
                    // Use the default object validation
                    if (!isObjectValidatable && !type.IsEnum)
                    {
                        ValidationContext validationContext = new(obj, serviceProvider, items: null);
                        res &= Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);
                        if (obj is IValidatableObject validatable)
                        {
                            validationResults.AddRange(validatable.Validate(validationContext));
                            if (validationResults.Count > 0)
                            {
                                res = false;
                                if (!AddResults(results, allResults, validationResults, member)) return Finalize();
                            }
                        }
                    }
                    // Validate an enumeration value
                    if (type.IsEnum)
                    {
                        Type numericType = type.GetEnumUnderlyingType() ?? throw new InvalidProgramException($"Enumeration {type} without underlaying numeric type");
                        if (type.GetCustomAttribute<FlagsAttribute>() != null)
                        {
                            bool err;
                            object number,
                                undefinedValue = null!;
                            if (UnsignedNumericEnumTypes.Contains(numericType))
                            {
                                ulong allValues = 0,
                                    numericValue = (ulong)Convert.ChangeType(Convert.ChangeType(obj, numericType), typeof(ulong));
                                number = numericValue;
                                foreach (object v in Enum.GetValues(type)) allValues |= (ulong)Convert.ChangeType(Convert.ChangeType(v, numericType), typeof(ulong));
                                err = (numericValue & ~allValues) != 0;
                                if (err) undefinedValue = numericValue & ~allValues;
                            }
                            else
                            {
                                long allValues = 0,
                                    numericValue = (long)Convert.ChangeType(Convert.ChangeType(obj, numericType), typeof(long));
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
                    Type? itemType,// Property item type
                        keyType;// Property dictionary value key type
                    Type[] genericArguments;// Property value type generic arguments
                    bool propFailed = true,// If the property validation failed
                        loopCancelled = false,// If the property validation loop was cancelled
                        noItemValidation = (type.GetCustomAttribute<ItemNoValidationAttribute>(inherit: true)?.ArrayLevel ?? -1)
                            == info.ArrayLevel;// Property value item validation disabled?
                    NullabilityInfoContext nullabilityContext = new();// Context for nullable validation
                    NullabilityInfo nullabilityInfo;// Nullability info
                    foreach (PropertyInfo pi in from pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                    // Included
                                                where (members?.Contains(pi.Name) ?? true) &&
                                                    // Public getter
                                                    (pi.GetMethod?.IsPublic ?? false) &&
                                                    // Not an indexer
                                                    pi.GetIndexParameters().Length == 0 &&
                                                    // Not excluded
                                                    (
                                                        isObjectValidatable ||
                                                        !pi.GetCustomAttributes(inherit: true).Any(a => a is NoValidationAttribute)
                                                    ) &&
                                                    !pi.GetCustomAttributes(inherit: true).Any(a => a.GetType().FullName == VALIDATENEVER_ATTRIBUTE_TYPE)
                                                orderby pi.Name
                                                select pi)
                    {
                        // Break the loop, if requested
                        if (loopCancelled) break;
                        try
                        {
                            // Run event handlers
                            propFailed = true;
                            (cancelled, res, propFailed) = RaiseEvent(OnObjectPropertyValidation, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res, pi);
                            if (cancelled) continue;
                            // Get the full property member name and its value
                            memberName = member == null ? pi.Name : $"{member}.{pi.Name}";
                            value = pi.GetValue(obj);
                            // Default property validation
                            if (isObjectValidatable)
                            {
                                res &= Validator.TryValidateProperty(value, new(obj, serviceProvider, items: null) { MemberName = pi.Name }, validationResults);
                                if (pi.GetCustomAttributes(inherit: true).Any(a => a is NoValidationAttribute)) continue;
                            }
                            // Ensure a valid value (shouldn't be NULL, if the property type isn't nullable) and skip NULL values
                            nullabilityInfo = nullabilityContext.Create(pi.GetMethod!.ReturnParameter);
                            if (value == null)
                            {
                                if (!IsNullable(nullabilityInfo))
                                {
                                    res = false;
                                    validationResults.Add(new(
                                        $"Property {pi.Name} value is null, but the property type {pi.PropertyType} isn't nullable (a non-null value is required)",
                                        new string[] { pi.Name }
                                        ));
                                    (_, res, _) = RaiseEvent(OnObjectPropertyValidationFailed, info.Seen, obj, validationResults, allResults, member, throwOnError, members, res);
                                }
                                continue;
                            }
                            // Deep object validation
                            valueType = value.GetType();
                            if (valueType.GetCustomAttributes(inherit: true).Any(a => a is NoValidationAttribute || a.GetType().FullName == VALIDATENEVER_ATTRIBUTE_TYPE)) continue;
                            genericArguments = valueType.GetGenericArguments();
                            if (!noItemValidation)
                                if (AsDictionary(value, out keyType, out itemType) is IDictionary dict)
                                {
                                    res &= ValidateDictionary(info, pi, dict, valueType, keyType!, itemType!, nullabilityInfo, validationResults, serviceProvider, throwOnError);
                                    continue;
                                }
                                else if (value is Array arr && valueType.IsArray && valueType.GetElementType() != null)
                                {
                                    res &= ValidateList(info, pi, arr, valueType, valueType.GetElementType(), nullabilityInfo, validationResults, serviceProvider, throwOnError);
                                    continue;
                                }
                                else if (AsList(value, out itemType) is IList list)
                                {
                                    res &= ValidateList(info, pi, list, valueType, itemType, nullabilityInfo, validationResults, serviceProvider, throwOnError);
                                    continue;
                                }
                                else if (value is ICollection col)
                                {
                                    res &= ValidateList(info, pi, col, valueType, itemType, nullabilityInfo, validationResults, serviceProvider, throwOnError);
                                    if (valueType.IsValueType) continue;
                                }
                                else if (value is IEnumerable enumerable)
                                {
                                    res &= ValidateList(info, pi, enumerable, valueType, itemType: null, nullabilityInfo, validationResults, serviceProvider, throwOnError);
                                    if (valueType.IsValueType) continue;
                                }
                            res &= ValidateObject(info, value, validationResults, pi.Name, throwOnError, serviceProvider: serviceProvider);
                        }
                        catch (ObjectValidationException ex)
                        {
                            if (throwOnError) throw;
                            res = false;
                            validationResults.Add(new($"{VALIDATION_EXCEPTION_PREFIX}{pi.Name}: {ex}", new string[] { pi.Name }));
                        }
                        catch (Exception ex)
                        {
                            res = false;
                            validationResults.Add(new($"{VALIDATION_EXCEPTION_PREFIX}{pi.Name}: {ex}", new string[] { pi.Name }));
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
                if (seenIndex > 0) info.Seen.RemoveAt(seenIndex);
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
            if (member != null)
                for (int i = 0; i < validationResults.Count; i++)
                    validationResults[i] = new(
                        validationResults[i].ErrorMessage,
                        !validationResults[i].MemberNames.Any()
                            ? new string[] { member }
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
            for (Type? type = obj.GetType(); type != null; type = type.BaseType)
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
            for (Type? type = obj.GetType(); type != null; type = type.BaseType)
            {
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>)) continue;
                isList = true;
                itemType = type.GetGenericArguments()[0];
                break;
            }
            return isList ? obj as IList : null;
        }
    }
}
