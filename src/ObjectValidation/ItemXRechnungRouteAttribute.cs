namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item XRechnung route validation attribute
    /// </summary>
    public class ItemXRechnungRouteAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemXRechnungRouteAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item)
            : base(target, new XRechnungRouteAttribute(normalize))
            { }
    }
}
