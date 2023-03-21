using CountryValidation;
using CountryValidation.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item ZIP code validation attribute
    /// </summary>
    public class ItemZipCodeAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemZipCodeAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new ZipCodeAttribute(country)) { }
    }
}
