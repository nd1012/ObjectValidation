using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for a validation attribute
    /// </summary>
    public abstract class ValidationAttributeBase : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ValidationAttributeBase() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        protected ValidationAttributeBase(in string errorMessage) : base(errorMessage) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorMessageAccessor">Error message accessor</param>
        protected ValidationAttributeBase(in Func<string> errorMessageAccessor) : base(errorMessageAccessor) { }

        /// <summary>
        /// Error message formatter
        /// </summary>
        public static ErrorMessage_Delegate ErrorMessageFormatter { get; set; }
            = (attr, context, message) => attr.ErrorMessage is null
                ? context.MemberName is null
                    ? message
                    : $"{context.MemberName}: {message}"
                : context.MemberName is null
                    ? attr.ErrorMessage
                    : attr.FormatErrorMessage(context.MemberName);

        /// <summary>
        /// Get the error message
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual string GetErrorMessage(in ValidationContext context, in string message) => ErrorMessageFormatter(this, context, message);

        /// <summary>
        /// Delegate for an error message provider
        /// </summary>
        /// <param name="attr">Attribute</param>
        /// <param name="context">Context</param>
        /// <param name="message">Message</param>
        /// <returns>Message</returns>
        public delegate string ErrorMessage_Delegate(ValidationAttribute attr, ValidationContext context, string message);
    }
}
