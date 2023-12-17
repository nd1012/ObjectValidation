using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Credit card item attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemCreditCardAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new CreditCardAttribute())
    {
    }
}
