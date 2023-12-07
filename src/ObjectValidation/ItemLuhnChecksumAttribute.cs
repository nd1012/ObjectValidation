namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item Luhn checksum validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemLuhnChecksumAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new LuhnChecksumAttribute(normalize))
    {
    }
}
