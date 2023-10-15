using System.ComponentModel.DataAnnotations;

//TODO .NET 8: Remove

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for validating allowed values
    /// </summary>
    public class AllowedValuesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Allowed values</param>
        public AllowedValuesAttribute(params object?[] values) : base() => AllowedValues = values;

        /// <summary>
        /// Allowed values
        /// </summary>
        public object?[] AllowedValues { get; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            => AllowedValues.Contains(value)
                ? null
                : new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Value isn't allowed" : $"{validationContext.MemberName}: Value isn't allowed"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
    }
}
