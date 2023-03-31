namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN item validation attribute
    /// </summary>
    public class ItemAbaRtnAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normalize">Normalize?</param>
        /// <param name="target">Validation target</param>
        public ItemAbaRtnAttribute(bool normalize = true, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new AbaRtnAttribute(normalize)) { }

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
