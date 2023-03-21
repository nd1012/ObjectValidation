namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item currency ISO 4217 code validation attribute
    /// </summary>
    public class ItemCurrencyAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="target">Validation target</param>
        public ItemCurrencyAttribute(CurrencyFormats format = CurrencyFormats.AlphabeticCode, ItemValidationTargets target = ItemValidationTargets.Item)
            : base(target, new CurrencyAttribute(format))
            { }
    }
}
