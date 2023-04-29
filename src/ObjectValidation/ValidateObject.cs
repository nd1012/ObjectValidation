using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Object validation methods
    /// </summary>
    public static class ValidateObject
    {
        /// <summary>
        /// Log message handler
        /// </summary>
        public static Logger_Delegate Logger { get; set; } = (message) => Debug.WriteLine(message);

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
            List<ValidationResult>? results = null,
            string? member = null,
            bool throwOnError = false,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
            => obj == null || ValidationExtensions.ValidateObject(new(), obj, results, member, throwOnError, members, serviceProvider);

        /// <summary>
        /// Ensure a valid object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="errorHandler">Error handler</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>If the object is valid</returns>
        public static bool EnsureValidObject(
            this object obj,
            Func<object, List<ValidationResult>, bool> errorHandler,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
        {
            List<ValidationResult> results = new();
            return obj.TryValidateObject(results, members: members, serviceProvider: serviceProvider) || errorHandler(obj, results);
        }

        /// <summary>
        /// Get a valid object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object</param>
        /// <param name="errorHandler">Error handler</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Valid object</returns>
        public static T GetValidObject<T>(
            this T obj,
            Func<T, List<ValidationResult>, T> errorHandler,
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            List<ValidationResult> results = new();
            return obj.TryValidateObject(results, members: members, serviceProvider: serviceProvider)
                ? obj
                : errorHandler(obj, results);
        }

        /// <summary>
        /// Delegate for a logger handler
        /// </summary>
        /// <param name="message"></param>
        public delegate void Logger_Delegate(string message);
    }
}
