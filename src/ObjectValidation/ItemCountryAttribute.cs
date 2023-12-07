namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item country code validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemCountryAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target,new CountryAttribute())
    {
    }
}
