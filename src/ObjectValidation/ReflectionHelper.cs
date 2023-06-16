using System.Collections.Concurrent;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Reflection helper
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// CreateGetterDelegate method
        /// </summary>
        internal static readonly MethodInfo CreateGetterDelegateMethod;
        /// <summary>
        /// Getter cache
        /// </summary>
        internal static readonly ConcurrentDictionary<int, Func<object?, object?>> GetterCache;
        /// <summary>
        /// Item validation attribute cache
        /// </summary>
        internal static readonly ConcurrentDictionary<int, IItemValidationAttribute[]> ItemValidationAttributeCache;
        /// <summary>
        /// <see cref="PropertyInfo"/> cache
        /// </summary>
        internal static readonly ConcurrentDictionary<int, PropertyInfo[]> PropertyInfoCache;
        /// <summary>
        /// <see cref="Attribute"/> cache
        /// </summary>
        internal static readonly ConcurrentDictionary<int, Attribute[]> AttributeCache;

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
        internal static Func<object?, object?>? GetGetterDelegate(this PropertyInfo pi)
            => pi.CanRead
                ? GetterCache.GetOrAdd(
                    pi.GetHashCode(),
                    (key) => (Func<object?, object?>)CreateGetterDelegateMethod.MakeGenericMethod(pi.DeclaringType!, pi.PropertyType)
                        .Invoke(obj: null, new object[] { pi.GetMethod!.CreateDelegate(typeof(Func<,>).MakeGenericType(pi.DeclaringType!, pi.PropertyType)) })!
                    )
                : null;

        /// <summary>
        /// Get item validation attributes
        /// </summary>
        /// <param name="cap">Object</param>
        /// <returns>Attributes</returns>
        internal static IItemValidationAttribute[] GetItemValidationAttributes(this ICustomAttributeProvider cap)
            => ItemValidationAttributeCache.GetOrAdd(
                cap.GetHashCode(),
                (key) => GetCustomAttributesCached(cap).Where(a => a is IItemValidationAttribute).Cast<IItemValidationAttribute>().ToArray()
                );

        /// <summary>
        /// Get cached <see cref="PropertyInfo"/>
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Properties</returns>
        internal static PropertyInfo[] GetPropertiesCached(this Type type)
            => PropertyInfoCache.GetOrAdd(
                type.GetHashCode(),
                (key) => type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.GetMethod!.IsPublic && p.GetIndexParameters().Length == 0).ToArray()
                );

        /// <summary>
        /// Get cached custom attributes
        /// </summary>
        /// <param name="cap">Object</param>
        /// <returns>Attributes</returns>
        internal static Attribute[] GetCustomAttributesCached(this ICustomAttributeProvider cap)
            => AttributeCache.GetOrAdd(
                cap.GetHashCode(),
                (key) => cap.GetCustomAttributes(inherit: true).Cast<Attribute>().ToArray()
                );

        /// <summary>
        /// Create a getter delegate
        /// </summary>
        /// <typeparam name="tObject">Object type</typeparam>
        /// <typeparam name="tValue">Value type</typeparam>
        /// <param name="getter">Getter</param>
        /// <returns>Getter delegate</returns>
        private static Func<object?, object?> CreateGetterDelegate<tObject, tValue>(Func<tObject?, tValue?> getter) => (obj) => getter((tObject)obj!);
    }
}
