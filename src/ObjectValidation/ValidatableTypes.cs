using System.Collections.Concurrent;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Validatable types
    /// </summary>
    public static class ValidatableTypes
    {
        /// <summary>
        /// Types which are forced to be validated
        /// </summary>
        public static readonly ConcurrentBag<Type> ForcedTypes;
        /// <summary>
        /// Types which are denied to be validated
        /// </summary>
        public static readonly ConcurrentBag<Type> DeniedTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        static ValidatableTypes()
        {
            ForcedTypes = new();
            DeniedTypes = new(new Type[] { typeof(string), typeof(object), typeof(IQueryable<>), typeof(Type), typeof(Stream) });
        }

        /// <summary>
        /// Determine if a type is validatable
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Is validatable?</returns>
        public static bool IsTypeValidatable(Type type)
        {
            Type? nullableType = Nullable.GetUnderlyingType(type);// Underlying nullable type
            bool res = nullableType == null || IsTypeValidatable(nullableType);// Result
            if (res)
            {
                Type? gtd = type.IsGenericType ? type.GetGenericTypeDefinition() : null;// Generic type definition
                res = !DeniedTypes.Contains(type) && // Not denied
                    ( // Not denied generic type definition
                        gtd == null ||
                        !DeniedTypes.Contains(gtd)
                    ) &&
                    ( // Allowed
                        ForcedTypes.Contains(type) || // Forced
                        ( // Forced generic type definition
                            gtd != null &&
                            ForcedTypes.Contains(gtd)
                        ) ||
                        !( // Other type restrictions
                            (type.IsValueType && !type.IsEnum) || // Not a non-enum value type
                            type.IsArray || // Not an array
                            type.GetCustomAttribute<NoValidationAttribute>(inherit: true) != null // Not ignored
                        )
                    );
                if (res) return res;// Validate, if validatable in this moment
            }
            // Make it possible to make a conditional exception for an usually not validated type
            ValidatableTypesEventArgs e = new(type);
            OnIsTypeValidatable?.Invoke(e);
#if DEBUG
            if (!e.IsValidatable) ValidateObject.Logger($"{type} skipped for validation");
#endif
            return e.IsValidatable;
        }

        /// <summary>
        /// Delegate for a <see cref="OnIsTypeValidatable"/> event
        /// </summary>
        /// <param name="e">Event arguments</param>
        public delegate void TypeValidatable_Delegate(ValidatableTypesEventArgs e);

        /// <summary>
        /// Raised when determining if a type is validatable (to make a conditional exception for otherwise not validated types)
        /// </summary>
        public static event TypeValidatable_Delegate? OnIsTypeValidatable;
    }
}
