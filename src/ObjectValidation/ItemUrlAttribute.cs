using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item URL attribute
    /// </summary>
    public class ItemUrlAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemUrlAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new UrlAttribute()) { }
    }
}
