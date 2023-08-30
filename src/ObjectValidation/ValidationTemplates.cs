using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Object validation templates
    /// </summary>
    public static class ValidationTemplates
    {
        /// <summary>
        /// Property validation templates (key is the template name, value a list of validation attributes to apply)
        /// </summary>
        public static readonly Dictionary<string, List<ValidationAttribute>> PropertyValidations = new();

        /// <summary>
        /// Item validation templates (key is the template name, value a list of validation attributes to apply)
        /// </summary>
        public static readonly Dictionary<string, List<IItemValidationAttribute>> ItemValidations = new();
    }
}
