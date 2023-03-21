namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item Luhn checksum validation attribute
    /// </summary>
    public class ItemLuhnChecksumAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemLuhnChecksumAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new LuhnChecksumAttribute(normalize)) { }
    }
}
