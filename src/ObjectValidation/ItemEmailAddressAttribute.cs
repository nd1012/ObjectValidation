using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item email address attribute
    /// </summary>
    public class ItemEmailAddressAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemEmailAddressAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new EmailAddressAttribute()) { }
    }
}
