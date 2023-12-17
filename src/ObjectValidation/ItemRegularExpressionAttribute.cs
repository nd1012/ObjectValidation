using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item regular expression attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="pattern">Pattern</param>
    /// <param name="target">Validation target</param>
    public class ItemRegularExpressionAttribute(string pattern, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new RegularExpressionAttribute(pattern))
    {
    }
}
