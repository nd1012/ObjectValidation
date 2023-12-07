namespace wan24.ObjectValidation
{
    /// <summary>
    /// BIC item validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemBicAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new IbanAttribute(normalize))
    {
    }
}
