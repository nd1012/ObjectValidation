using CountryValidation;
using CountryValidation.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item company TIN validation attribute
    /// </summary>
    public class ItemCompanyTINAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemCompanyTINAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new CompanyTINAttribute(country)) { }
    }
}
