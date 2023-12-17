using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item email address attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemEmailAddressAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new EmailAddressAttribute())
    {
    }
}
