namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item denied special string/character/list characters validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="allowUsualTextFormatting">If to allow special text characters (tabulator and line break)</param>
    /// <param name="useUnicode">If to use Unicode (otherwise 7bit ASCII)</param>
    /// <param name="target">Validation target</param>
    /// <param name="additionalDenied">Additional denied characters</param>
    public class ItemDenyCharactersAttribute(
        bool allowUsualTextFormatting = false,
        bool useUnicode = true,
        ItemValidationTargets target = ItemValidationTargets.Item,
        params char[] additionalDenied
        )
        : ItemValidationAttribute(target, new DenyCharactersAttribute(allowUsualTextFormatting, useUnicode, additionalDenied))
    {
        /// <summary>
        /// If to deny all control characters (<see cref="char.IsControl(char)"/>)
        /// </summary>
        public bool DenyAllControl
        {
            get => ((DenyCharactersAttribute)ValidationAttribute).DenyAllControl;
            set => ((DenyCharactersAttribute)ValidationAttribute).DenyAllControl = value;
        }
    }
}
