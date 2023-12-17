namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN item validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="normalize">Normalize?</param>
    /// <param name="target">Validation target</param>
    public class ItemAbaRtnAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new AbaRtnAttribute(normalize))
    {
        /// <summary>
        /// Supported ABA RTN formats
        /// </summary>
        public AbaFormats Format
        {
            get => ((AbaRtnAttribute)ValidationAttribute).Format;
            set => ((AbaRtnAttribute)ValidationAttribute).Format = value;
        }
    }
}
