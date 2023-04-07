using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for custom item validation using a public static method
    /// </summary>
    public class ItemCustomValidationAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="method">Static method name</param>
        /// <param name="target">Validation target</param>
        public ItemCustomValidationAttribute(Type type, string method, ItemValidationTargets target = ItemValidationTargets.Item)
            : base(target, new CustomValidationAttribute(type, method))
        { }
    }
}
