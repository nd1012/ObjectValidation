namespace wan24.ObjectValidation
{
    /// <summary>
    /// IBAN item validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemIbanAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target,new IbanAttribute(normalize))
    {
    }
}
