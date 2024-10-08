﻿namespace wan24.ObjectValidation
{
    /// <summary>
    /// Validatable types
    /// </summary>
    public static class ValidatableTypes
    {
        /// <summary>
        /// Types which are forced to be validated
        /// </summary>
        public static readonly HashSet<Type> ForcedTypes;
        /// <summary>
        /// Types (also inherited) which are forced to be validated
        /// </summary>
        public static readonly HashSet<Type> ForcedTypesInheritable;
        /// <summary>
        /// Types which are denied to be validated
        /// </summary>
        public static readonly HashSet<Type> DeniedTypes;
        /// <summary>
        /// Types (also inherited) which are denied to be validated
        /// </summary>
        public static readonly HashSet<Type> DeniedTypesInheritable;

        /// <summary>
        /// Constructor
        /// </summary>
        static ValidatableTypes()
        {
            ForcedTypes = [];
            ForcedTypesInheritable = [];
            DeniedTypes = new([typeof(string), typeof(object), typeof(IQueryable<>)]);
            DeniedTypesInheritable = new([typeof(Type), typeof(Stream)]);
        }

        /// <summary>
        /// Determine if a type is validatable
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Is validatable?</returns>
        public static bool IsTypeValidatable(Type type)
        {
            Type? nullableType = Nullable.GetUnderlyingType(type);// Underlying nullable type
            bool res = nullableType is null || IsTypeValidatable(nullableType);// Result
            if (res)
            {
                Type? gtd = type.IsGenericType ? type.GetGenericTypeDefinition() : null;// Generic type definition
                res = ForcedTypes.Contains(type) || // Forced
                    ForcedTypesInheritable.Any(t => t.IsAssignableFrom(type)) ||
                    ( // Forced generic type definition
                        gtd is not null &&
                        (
                            ForcedTypes.Contains(gtd) ||
                            ForcedTypesInheritable.Any(t => t.IsAssignableFrom(gtd))
                        )
                    ) ||
                    (
                        !DeniedTypes.Contains(type) && // Not denied
                        !DeniedTypesInheritable.Any(t => t.IsAssignableFrom(type)) &&
                        ( // Not denied generic type definition
                            gtd is null ||
                            (
                                !DeniedTypes.Contains(gtd) &&
                                !DeniedTypesInheritable.Any(t => t.IsAssignableFrom(gtd))
                            )
                        ) &&
                        !( // Other type restrictions
                            (type.IsValueType && !type.IsEnum) || // Not a non-enum value type
                            type.IsArray || // Not an array
                            type.GetCustomAttributesCached().Any(a => a is NoValidationAttribute)// Not ignored
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
