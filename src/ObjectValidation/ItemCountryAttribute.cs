namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item country code validation attribute
    /// </summary>
    public class ItemCountryAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemCountryAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target,new CountryAttribute()) { }
    }
}
