using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item required attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemRequiredAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new RequiredAttribute())
    {
    }
}
