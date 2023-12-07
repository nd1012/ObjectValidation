using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Maximum item length attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="len">Length</param>
    /// <param name="target">Validation target</param>
    public class ItemMaxLengthAttribute(int len, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new MaxLengthAttribute(len))
    {
    }
}
