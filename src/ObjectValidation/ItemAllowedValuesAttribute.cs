namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for validating allowed item values
    /// </summary>
    public class ItemAllowedValuesAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="values">Allowed values</param>
        public ItemAllowedValuesAttribute(params object?[] values)
            : base(ItemValidationTargets.Item, new AllowedValuesAttribute(values))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        /// <param name="values">Allowed values</param>
        public ItemAllowedValuesAttribute(ItemValidationTargets target, params object?[] values)
            : base(target, new AllowedValuesAttribute(values))
        { }
    }
}
