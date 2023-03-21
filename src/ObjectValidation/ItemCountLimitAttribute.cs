namespace wan24.ObjectValidation
{
    /// <summary>
    /// Count limitation attribute
    /// </summary>
    public class ItemCountLimitAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="max">Maximum</param>
        /// <param name="target">Validation target</param>
        public ItemCountLimitAttribute(long max, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new CountLimitAttribute(max)) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <param name="target">Validation target</param>
        public ItemCountLimitAttribute(long min, long max, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new CountLimitAttribute(min, max)) { }
    }
}
