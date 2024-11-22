namespace wan24.ObjectValidation
{
    /// <summary>
    /// Single line text validation attribute (denies control characters - tabulator optional)
    /// </summary>
    /// <param name="allowTab">If to allow tabulator</param>
    public class SingleLineAttribute(bool allowTab = false) : DenyCharactersAttribute(allowUsualTextFormatting: true, useUnicode: true, allowTab ? [] : ['\t'])
    {
    }
}
