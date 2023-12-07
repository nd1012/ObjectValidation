namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item amount validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="currency">Currency</param>
    /// <param name="target">Target validation</param>
    public class ItemAmountAttribute(string currency, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new AmountAttribute(currency))
    {
    }
}
