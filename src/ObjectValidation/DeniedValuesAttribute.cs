using System.ComponentModel.DataAnnotations;

//TODO .NET 8: Remove

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for validating against denied values
    /// </summary>
    public class DeniedValuesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Denied values</param>
        public DeniedValuesAttribute(params object?[] values) : base() => DeniedValues = values;

        /// <summary>
        /// Denied values
        /// </summary>
        public object?[] DeniedValues { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            => DeniedValues.Contains(value)
                ? new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Value is denied" : $"{validationContext.MemberName}: Value is denied"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    )
                : null;
    }
}
