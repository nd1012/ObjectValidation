using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item phone number attribute
    /// </summary>
    public class ItemPhoneAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemPhoneAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new PhoneAttribute()) { }
    }
}
