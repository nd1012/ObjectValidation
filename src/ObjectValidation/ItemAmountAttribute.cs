namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item amount validation attribute
    /// </summary>
    public class ItemAmountAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="target">Target validation</param>
        public ItemAmountAttribute(string currency, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new AmountAttribute(currency)) { }
    }
}
