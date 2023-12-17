using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Minimum item length attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="len">Length</param>
    /// <param name="target">Validation target</param>
    public class ItemMinLengthAttribute(int len, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new MinLengthAttribute(len))
    {
    }
}
