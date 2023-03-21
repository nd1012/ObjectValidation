namespace wan24.ObjectValidation
{
    /// <summary>
    /// IBAN item validation attribute
    /// </summary>
    public class ItemIbanAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemIbanAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item):base(target,new IbanAttribute(normalize)) { }
    }
}
