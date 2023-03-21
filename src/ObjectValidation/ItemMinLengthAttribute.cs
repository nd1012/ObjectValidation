using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Minimum item length attribute
    /// </summary>
    public class ItemMinLengthAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="len">Length</param>
        /// <param name="target">Validation target</param>
        public ItemMinLengthAttribute(int len, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new MinLengthAttribute(len)) { }
    }
}
