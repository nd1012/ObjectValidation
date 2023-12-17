namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item European VAT ID validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemEuVatIdAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new EuVatIdAttribute(normalize))
    {
    }
}
