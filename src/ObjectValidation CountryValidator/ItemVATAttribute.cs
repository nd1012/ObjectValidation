using CountryValidation;
using CountryValidation.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item VAT validation attribute
    /// </summary>
    public class ItemVATAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemVATAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new VATAttribute(country)) { }
    }
}
