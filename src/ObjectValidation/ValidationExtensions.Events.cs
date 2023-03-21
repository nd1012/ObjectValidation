using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    // Events
    public static partial class ValidationExtensions
    {
        /// <summary>
        /// Raise an event
        /// </summary>
        /// <param name="delegates">Delegates</param>
        /// <param name="seen">Seen objects</param>
        /// <param name="obj">Object</param>
        /// <param name="validationResults">Current validation results</param>
        /// <param name="allResults">All current validation results</param>
        /// <param name="member">Member name</param>
        /// <param name="throwOnError">Throw a <see cref="ObjectValidationException"/> on any error?</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="res">Current result</param>
        /// <param name="pi">Current property</param>
        /// <returns>If the event was cancelled, the new result, and if the validation failed during the event</returns>
        internal static (bool Cancelled, bool NewResult, bool Failed) RaiseEvent(
            ObjectValidation_Delegate? delegates,
            List<object> seen,
            object obj,
            List<ValidationResult> validationResults,
            IReadOnlyList<ValidationResult> allResults,
            string? member,
            bool throwOnError,
            IEnumerable<string>? members,
            bool res,
            PropertyInfo? pi = null
            )
        {
            if (delegates == null) return (Cancelled: false, NewResult: res, Failed: false);
            ObjectValidationEventArgs e = new(seen, obj, validationResults, allResults, member, throwOnError, members, res, pi);
            delegates(e);
            if (!res && e.Result) throw new InvalidOperationException("Validation failed state can't be overridden");
            return (Cancelled: e.Cancel, NewResult: e.Result, Failed: res != e.Result);
        }

        /// <summary>
        /// Delegate for object validation event handlers
        /// </summary>
        /// <param name="e">Event arguments</param>
        public delegate void ObjectValidation_Delegate(ObjectValidationEventArgs e);

        /// <summary>
        /// Raised on object validation
        /// </summary>
        public static event ObjectValidation_Delegate? OnObjectValidation;

        /// <summary>
        /// Raised on object property validation
        /// </summary>
        public static event ObjectValidation_Delegate? OnObjectPropertyValidation;

        /// <summary>
        /// Raised on a failed object validation
        /// </summary>
        public static event ObjectValidation_Delegate? OnObjectValidationFailed;

        /// <summary>
        /// Raised on a failed object property validation
        /// </summary>
        public static event ObjectValidation_Delegate? OnObjectPropertyValidationFailed;
    }
}
