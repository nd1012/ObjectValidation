using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item URL attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemUrlAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new UrlAttribute())
    {
    }
}
