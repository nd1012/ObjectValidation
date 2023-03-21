namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for properties which serves dictionaries or lists, but items shouldn't be validated
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class,AllowMultiple = true, Inherited = true)]
    public class ItemNoValidationAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemNoValidationAttribute(ItemValidationTargets target = ItemValidationTargets.Item) :base(target, new NoValidationAttribute()) { }
    }
}
