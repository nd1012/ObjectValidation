using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item string length attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="maxLen">Maximum length</param>
    /// <param name="target">Validation target</param>
    public class ItemStringLengthAttribute(int maxLen, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new StringLengthAttribute(maxLen))
    {
    }
}
