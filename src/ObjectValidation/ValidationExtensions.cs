﻿using System.ComponentModel.DataAnnotations;

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
        /// Validate an object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="results">Results</param>
        /// <param name="member">Member name</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="serviceProvider">Service provider</param>
        /// <returns>Object</returns>
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation</exception>
        public static T ValidateObject<T>(this T obj, out List<ValidationResult> results, string? member = null, IEnumerable<string>? members = null, IServiceProvider? serviceProvider = null)
            where T : notnull
        {
            TryValidateObject(obj, results = new(), member, throwOnError: true, members, serviceProvider);
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
        public static T ValidateObject<T>(this T obj, List<ValidationResult> results, string? member = null, IEnumerable<string>? members = null, IServiceProvider? serviceProvider = null)
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
        /// <exception cref="ObjectValidationException">Thrown on error during an object validation (won't be thrown, if <paramref name="throwOnError"/> is <see langword="false"/>, which is the default)</exception>
        public static bool TryValidateObject(
            this object obj, 
            out List<ValidationResult> results, 
            string? member = null, 
            bool throwOnError = false, 
            IEnumerable<string>? members = null, 
            IServiceProvider? serviceProvider = null
            )
            => TryValidateObject(obj, results = new(), member, throwOnError, members, serviceProvider);

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
        /// Determine if validation results contain a validation exception
        /// </summary>
        /// <param name="results">Results</param>
        /// <returns>Contains a validation exception?</returns>
        public static bool HasValidationException(this IEnumerable<ValidationResult> results) => results.Any(r => r.ErrorMessage?.StartsWith(VALIDATION_EXCEPTION_PREFIX) ?? false);
    }
}
