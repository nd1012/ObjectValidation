using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Credit card item attribute
    /// </summary>
    public class ItemCreditCardAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemCreditCardAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new CreditCardAttribute()) { }
    }
}
