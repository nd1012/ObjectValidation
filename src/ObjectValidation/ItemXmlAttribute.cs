namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item XML validation attribute
    /// </summary>
    public class ItemXmlAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target">Validation target</param>
        public ItemXmlAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new XmlAttribute()) { }
    }
}
