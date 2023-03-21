using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Thrown on error during an object validation
    /// </summary>
    public class ObjectValidationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectValidationException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public ObjectValidationException(string? message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public ObjectValidationException(string? message, Exception? inner) : base(message, inner) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="results">Validation results</param>
        public ObjectValidationException(IEnumerable<ValidationResult> results) : base(results.LastOrDefault()?.ErrorMessage) => Results = results;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="results">Validation results</param>
        /// <param name="message">Message</param>
        public ObjectValidationException(IEnumerable<ValidationResult> results, string? message) : base(message) => Results = results;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="results">Validation results</param>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public ObjectValidationException(IEnumerable<ValidationResult> results, string? message, Exception? inner) : base(message, inner) => Results = results;

        /// <summary>
        /// Validation results
        /// </summary>
        public IEnumerable<ValidationResult>? Results { get; }
    }
}
