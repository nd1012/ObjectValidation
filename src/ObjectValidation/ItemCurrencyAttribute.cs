namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item currency ISO 4217 code validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="format">Format</param>
    /// <param name="target">Validation target</param>
    public class ItemCurrencyAttribute(CurrencyFormats format = CurrencyFormats.AlphabeticCode, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new CurrencyAttribute(format))
    {
    }
}
