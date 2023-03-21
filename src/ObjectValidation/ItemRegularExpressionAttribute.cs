using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item regular expression attribute
    /// </summary>
    public class ItemRegularExpressionAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <param name="target">Validation target</param>
        public ItemRegularExpressionAttribute(string pattern, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new RegularExpressionAttribute(pattern)) { }
    }
}
