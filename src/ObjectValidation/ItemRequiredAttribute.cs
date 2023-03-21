using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item required attribute
    /// </summary>
    public class ItemRequiredAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemRequiredAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new RequiredAttribute()) { }
    }
}
