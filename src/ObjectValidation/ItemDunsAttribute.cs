namespace wan24.ObjectValidation
{
    /// <summary>
    /// D-U-N-S number item validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemDunsAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new DunsAttribute(normalize))
    {
    }
}
