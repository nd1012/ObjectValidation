using CountryValidation;
using CountryValidation.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item SSN validation attribute
    /// </summary>
    public class ItemSSNAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemSSNAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item):base(target, new SSNAttribute(country)) { }
    }
}
