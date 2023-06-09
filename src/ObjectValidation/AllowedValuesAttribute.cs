﻿using System.ComponentModel.DataAnnotations;

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
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Value isn't allowed" : $"{validationContext.MemberName}: Value isn't allowed"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
    }
}
