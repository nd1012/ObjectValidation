namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item XRechnung route validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemXRechnungRouteAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemValidationAttribute(target, new XRechnungRouteAttribute(normalize))
    {
    }
}
