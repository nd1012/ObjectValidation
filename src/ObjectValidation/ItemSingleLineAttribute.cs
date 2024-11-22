namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item single line text validation attribute
    /// </summary>
    /// <param name="allowTab">If to allow tabulator</param>
    /// <param name="target">Validation target</param>
    public class ItemSingleLineAttribute(bool allowTab = false, ItemValidationTargets target = ItemValidationTargets.Item)
        : ItemDenyCharactersAttribute(
            allowUsualTextFormatting: true,
            useUnicode: true,
            target,
            allowTab ? [] : ['\t']
            )
    {
    }
}
