using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Reflection helper
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Never validate attribute full type (ASP.NET)
        /// </summary>
        internal const string VALIDATENEVER_ATTRIBUTE_TYPE = "Microsoft.AspNetCore.Mvc.ModelBinding.Validation.NeverValidateAttribute";

        /// <summary>
        /// CreateGetterDelegate method
        /// </summary>
        private static readonly MethodInfo CreateGetterDelegateMethod;
        /// <summary>
        /// Getter cache
        /// </summary>
        private static readonly ConcurrentDictionary<int, PropertyGetter_Delegate> GetterCache;
        /// <summary>
        /// Item validation attribute cache
        /// </summary>
        private static readonly ConcurrentDictionary<int, FrozenSet<IItemValidationAttribute>> ItemValidationAttributeCache;
        /// <summary>
        /// <see cref="PropertyInfo"/> cache
        /// </summary>
        private static readonly ConcurrentDictionary<int, FrozenSet<PropertyInfo>> PropertyInfoCache;
        /// <summary>
        /// <see cref="Attribute"/> cache
        /// </summary>
        private static readonly ConcurrentDictionary<int, FrozenSet<Attribute>> AttributeCache;

        /// <summary>
        /// Static constructor
        /// </summary>
        static ReflectionHelper()
        {
            CreateGetterDelegateMethod = typeof(ReflectionHelper).GetMethod(nameof(CreateGetterDelegate), BindingFlags.NonPublic | BindingFlags.Static)
                ?? throw new InvalidProgramException($"Failed to reflect {typeof(ReflectionHelper)}.{nameof(CreateGetterDelegate)}");
            GetterCache = new();
            ItemValidationAttributeCache = new();
            PropertyInfoCache = new();
            AttributeCache = new();
        }

        /// <summary>
        /// Get the property getter
        /// </summary>
        /// <param name="pi">Property</param>
        /// <returns>Getter</returns>
        public static PropertyGetter_Delegate? GetGetterDelegate(this PropertyInfo pi)
            => pi.CanRead
                ? GetterCache.GetOrAdd(
                    pi.GetHashCode(),
                    (key) => (PropertyGetter_Delegate)CreateGetterDelegateMethod.MakeGenericMethod(pi.DeclaringType!, pi.PropertyType)
                        .Invoke(obj: null, [pi.GetMethod!.CreateDelegate(typeof(Func<,>).MakeGenericType(pi.DeclaringType!, pi.PropertyType))])!
                    )
                : null;

        /// <summary>
        /// Get item validation attributes
        /// </summary>
        /// <param name="cap">Object</param>
        /// <returns>Attributes</returns>
        public static FrozenSet<IItemValidationAttribute> GetItemValidationAttributes(this ICustomAttributeProvider cap)
            => ItemValidationAttributeCache.GetOrAdd(
                cap.GetHashCode(),
                (key) => GetCustomAttributesCached(cap).Where(a => a is IItemValidationAttribute).Cast<IItemValidationAttribute>().ToFrozenSet()
                );

        /// <summary>
        /// Get cached <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Properties</returns>
        public static FrozenSet<PropertyInfo> GetPropertiesCached(this Type type)
            => PropertyInfoCache.GetOrAdd(
                type.GetHashCode(),
                (key) => type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(
                        p => (p.GetMethod?.IsPublic ?? false) && 
                            !p.PropertyType.IsByRef && 
                            !p.PropertyType.IsByRefLike && 
                            p.GetIndexParameters().Length == 0 &&
                            !p.GetCustomAttributesCached().Any(a => (a is NoValidationAttribute attr && attr.SkipNullValueCheck) || a.GetType().FullName == VALIDATENEVER_ATTRIBUTE_TYPE)
                        )
                    .ToFrozenSet()
                );

        /// <summary>
        /// Get cached custom attributes
        /// </summary>
        /// <param name="cap">Object</param>
        /// <returns>Attributes</returns>
        public static FrozenSet<Attribute> GetCustomAttributesCached(this ICustomAttributeProvider cap)
            => AttributeCache.GetOrAdd(
                cap.GetHashCode(),
                (key) => cap.GetCustomAttributes(inherit: true).Cast<Attribute>().ToFrozenSet()
                );

        /// <summary>
        /// Create a getter delegate
        /// </summary>
        /// <typeparam name="tObject">Object type</typeparam>
        /// <typeparam name="tValue">Value type</typeparam>
        /// <param name="getter">Getter</param>
        /// <returns>Getter delegate</returns>
        private static PropertyGetter_Delegate CreateGetterDelegate<tObject, tValue>(Func<tObject?, tValue?> getter) => (obj) => getter((tObject)obj!);

        /// <summary>
        /// Property getter delegate
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Property value</returns>
        public delegate object? PropertyGetter_Delegate(object? obj);
    }
}
