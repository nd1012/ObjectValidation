using CountryValidation;
using CountryValidation.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item person TIN validation attribute
    /// </summary>
    public class ItemPersonTINAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="country">Country</param>
        /// <param name="target">Validation target</param>
        public ItemPersonTINAttribute(Country country = Country.XX, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new PersonTINAttribute(country)) { }
    }
}
