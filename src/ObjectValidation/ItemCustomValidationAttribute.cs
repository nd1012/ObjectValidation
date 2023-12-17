using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for custom item validation using a public static method
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="type">Type</param>
    /// <param name="method">Static method name</param>
    /// <param name="target">Validation target</param>
    public class ItemCustomValidationAttribute(Type type, string method, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new CustomValidationAttribute(type, method))
    {
    }
}
