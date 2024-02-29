using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Validation extensions
    /// </summary>
    public static partial class ValidationExtensions
    {
        /// <summary>
        /// Validation exception prefix string
        /// </summary>
        public const string VALIDATION_EXCEPTION_PREFIX = $"Object validation exception: ";

        /// <summary>
        /// Maximum recursion depth (zero for no limit)
        /// </summary>
        public static int MaxDepth { get; set; } = 32;

        /// <summary>
        /// Maximum number of errors (zero for no limit)
        /// </summary>
        public static int MaxErrors { get; set; } = 200;

        /// <summary>
        /// Ignore errors when calling a public get-only-property getter?
        /// </summary>
        public static bool IgnoreGetOnlyErrors { get; set; } = true;

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
        public static T ValidateObject<T>(
            this T obj, out List<ValidationResult> results, 
            string? member = null, 
            IEnumerable<string>? members = null, 
            IServiceProvider? serviceProvider = null
            )
            where T : notnull
        {
            TryValidateObject(obj, results = [], member, throwOnError: true, members, serviceProvider);
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
        public static T ValidateObject<T>(
            this T obj, List<ValidationResult> results, 
            string? member = null, 
            IEnumerable<string>? members = null, 
            IServiceProvider? serviceProvider = null
            )
            where T : notnull
        {
            TryValidateObject(obj, results, member, throwOnError: true, members, serviceProvider);
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
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, 
        /// which is the default)</exception>
        public static bool TryValidateObject(
            this object obj, 
            out List<ValidationResult> results, 
            string? member = null, 
            bool throwOnError = false, 
            IEnumerable<string>? members = null, 
            IServiceProvider? serviceProvider = null
            )
            => TryValidateObject(obj, results = [], member, throwOnError, members, serviceProvider);

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
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, 
        /// which is the default)</exception>
        public static bool TryValidateObject(
            this object obj, 
            List<ValidationResult>? results = null, 
            string? member = null, 
            bool throwOnError = false, 
            IEnumerable<string>? members = null,
            IServiceProvider? serviceProvider = null
            )
            => ValidateObject(new(), obj, results, member, throwOnError, members, serviceProvider);

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
            List<ValidationResult> results = [];
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
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            List<ValidationResult> results = [];
            return obj.TryValidateObject(results, members: members, serviceProvider: serviceProvider)
                ? obj
                : errorHandler(obj, results);
        }

        /// <summary>
        /// Determine if validation results contain a validation exception
        /// </summary>
        /// <param name="results">Results</param>
        /// <returns>Contains a validation exception?</returns>
        public static bool HasValidationException(this IEnumerable<ValidationResult> results) => results.Any(r => r.ErrorMessage?.StartsWith(VALIDATION_EXCEPTION_PREFIX) ?? false);

        /// <summary>
        /// Create a validation result
        /// </summary>
        /// <param name="attr">Attribute</param>
        /// <param name="message">Error message</param>
        /// <param name="validationContext">Context</param>
        /// <returns><see cref="ValidationResult"/></returns>
        public static ValidationResult CreateValidationResult(this ValidationAttribute attr, in string message, in ValidationContext validationContext)
            => new(
                attr.ErrorMessage ?? (validationContext.MemberName is null ? message : $"{validationContext.MemberName}: {message}"),
                validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                );
    }
}
