namespace wan24.ObjectValidation
{
    /// <summary>
    /// BIC item validation attribute
    /// </summary>
    public class ItemBicAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemBicAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new IbanAttribute(normalize)) { }
    }
}
