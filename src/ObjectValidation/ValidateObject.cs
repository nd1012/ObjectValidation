using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Object validation methods
    /// </summary>
    public static class ValidateObject
    {
        /// <summary>
        /// Validate an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Object</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation</exception>
        public static T? ValidateNullableObject<T>(
            T? obj,
            out List<ValidationResult> results,
            string? member = null,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
        {
            TryValidateNullableObject(obj, results = new(), member, throwOnError: true, members, serviceProvider);
            return obj;
        }

        /// <summary>
        /// Validate an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Object</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation</exception>
        public static T? ValidateNullableObject<T>(T? obj, List<ValidationResult> results, string? member = null, IEnumerable<string>? members = null, IServiceProvider? serviceProvider = null)
        {
            TryValidateNullableObject(obj, results, member, throwOnError: true, members, serviceProvider);
            return obj;
        }

        /// <summary>
        /// Validate an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="throwOnError">Throw a <see cref="ObjectValidationException"/> on any error?</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Valid?</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, which is the default)</exception>
        public static bool TryValidateNullableObject(
            this object? obj,
            out List<ValidationResult> results,
            string? member = null,
            bool throwOnError = false,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
            => TryValidateNullableObject(obj, results = new(), member, throwOnError, members, serviceProvider);

        /// <summary>
        /// Validate an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="throwOnError">Throw a <see cref="ObjectValidationException"/> on any error?</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Valid?</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, which is the default)</exception>
        public static bool TryValidateNullableObject(
            this object? obj,
            List<ValidationResult>?
            results = null,
            string? member = null,
            bool throwOnError = false,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
            => obj == null || ValidationExtensions.ValidateObject(new(), obj, results, member, throwOnError, members, serviceProvider);
    }
}
