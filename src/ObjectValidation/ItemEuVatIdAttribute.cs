namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item European VAT ID validation attribute
    /// </summary>
    public class ItemEuVatIdAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemEuVatIdAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new EuVatIdAttribute(normalize)) { }
    }
}
