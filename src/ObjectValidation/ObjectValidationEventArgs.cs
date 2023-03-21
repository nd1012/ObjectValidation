using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Object validation event arguments
    /// </summary>
    public sealed class ObjectValidationEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="seen">Seen objects</param>
        /// <param name="obj">Object</param>
        /// <param name="validationResults">Current validation results</param>
        /// <param name="allResults">All current validation results</param>
        /// <param name="member">Member name</param>
        /// <param name="throwOnError">Throw a <see cref="ObjectValidationException"/> on any error?</param>
        /// <param name="members">Member names to validate</param>
        /// <param name="res">Current result</param>
        /// <param name="pi">Current property</param>
        public ObjectValidationEventArgs(
            List<object> seen, 
            object obj,
            List<ValidationResult> validationResults,
            IReadOnlyList<ValidationResult> allResults,
            string? member,
            bool throwOnError,
            IEnumerable<string>? members,
            bool res,
            PropertyInfo? pi = null
            ) : base()
        {
            Seen = seen;
            Object = obj;
            ValidationResults = validationResults;
            AllResults = allResults;
            Member = member;
            ThrowOnError = throwOnError;
            Members = members;
            OriginalResult = Result = res;
            Property = pi;
        }

        /// <summary>
        /// Seen objects
        /// </summary>
        public List<object> Seen { get; }

        /// <summary>
        /// Object
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// Current validation results
        /// </summary>
        public List<ValidationResult> ValidationResults { get; }

        /// <summary>
        /// All current validation results
        /// </summary>
        public IReadOnlyList<ValidationResult> AllResults { get; }

        /// <summary>
        /// Member name
        /// </summary>
        public string? Member { get; }

        /// <summary>
        /// Throw a <see cref="ObjectValidationException"/> on any error?
        /// </summary>
        public bool ThrowOnError { get; }

        /// <summary>
        /// Member names to validate
        /// </summary>
        public IEnumerable<string>? Members { get; }

        /// <summary>
        /// Original result
        /// </summary>
        public bool OriginalResult { get; }

        /// <summary>
        /// Current result
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Current property
        /// </summary>
        public PropertyInfo? Property { get; }
    }
}
