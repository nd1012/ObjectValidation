using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item length validation attribute
    /// </summary>
    public sealed class ItemLengthAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minimumLength">Min. length</param>
        /// <param name="maximumLength">Max. length</param>
        public ItemLengthAttribute(int minimumLength, int maximumLength) : base(new LengthAttribute(minimumLength, maximumLength)) { }
    }
}
