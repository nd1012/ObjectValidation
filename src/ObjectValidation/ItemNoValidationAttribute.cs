namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for properties which serves dictionaries or lists, but items shouldn't be validated (optional completely)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ItemNoValidationAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemNoValidationAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new NoValidationAttribute()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="skipNullValueCheck">Skip the <see langword="null"/> value check(s)?</param>
        /// <param name="target">Validation target</param>
        public ItemNoValidationAttribute(bool skipNullValueCheck, ItemValidationTargets target = ItemValidationTargets.Item)
            : base(target, new NoValidationAttribute(skipNullValueCheck))
        { }
    }
}
