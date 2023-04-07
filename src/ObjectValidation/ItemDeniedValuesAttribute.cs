namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for validating against denied item values
    /// </summary>
    public class ItemDeniedValuesAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Denied values</param>
        public ItemDeniedValuesAttribute(params object?[] values)
            : base(ItemValidationTargets.Item, new DeniedValuesAttribute(values))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        /// <param name="values">Denied values</param>
        public ItemDeniedValuesAttribute(ItemValidationTargets target, params object?[] values)
            : base(target, new DeniedValuesAttribute(values))
        { }
    }
}
