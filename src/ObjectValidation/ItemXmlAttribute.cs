namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item XML validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemXmlAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new XmlAttribute())
    {
    }
}
