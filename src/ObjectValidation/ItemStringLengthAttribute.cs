using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item string length attribute
    /// </summary>
    public class ItemStringLengthAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxLen">Maximum length</param>
        /// <param name="target">Validation target</param>
        public ItemStringLengthAttribute(int maxLen, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new StringLengthAttribute(maxLen)) { }
    }
}
