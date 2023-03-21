using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Maximum item length attribute
    /// </summary>
    public class ItemMaxLengthAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="len">Length</param>
        /// <param name="target">Validation target</param>
        public ItemMaxLengthAttribute(int len, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new MaxLengthAttribute(len)) { }
    }
}
