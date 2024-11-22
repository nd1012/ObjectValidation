using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Denied special string/character/list characters validation attribute
    /// </summary>
    public class DenyCharactersAttribute : ValidationAttributeBase
    {
        /// <summary>
        /// All 7bit ASCII control characters (0-31, 127)
        /// </summary>
        public static readonly ImmutableArray<char> SpecialCharacters;
        /// <summary>
        /// All Unicode control characters (0-31, 127-159)
        /// </summary>
        public static readonly ImmutableArray<char> SpecialUnicodeCharacters;
        /// <summary>
        /// Non-text formatting 7bit ASCII control characaters (0-31, 127; excludes tabulator and line break (9, 10, 13))
        /// </summary>
        public static readonly ImmutableArray<char> NonTextFormattingSpecialCharacters;
        /// <summary>
        /// Non-text formatting Unicode control characaters (0-31, 127-159; excludes tabulator and line break (9, 10, 13))
        /// </summary>
        public static readonly ImmutableArray<char> NonTextFormattingSpecialUnicodeCharacters;

        /// <summary>
        /// Static constructor
        /// </summary>
        static DenyCharactersAttribute()
        {
            HashSet<char> characters = [];
            for (int i = 0; i < 32; characters.Add((char)i), i++) ;
            characters.Add((char)127);// DEL
            SpecialCharacters = [.. characters];
            for (int i = 128; i < 160; characters.Add((char)i), i++) ;
            SpecialUnicodeCharacters = [.. characters];
            characters.Remove((char)9);// TAB
            characters.Remove((char)10);// LF
            characters.Remove((char)13);// CR
            NonTextFormattingSpecialUnicodeCharacters = [.. characters];
            for (int i = 128; i < 160; characters.Remove((char)i), i++) ;
            NonTextFormattingSpecialCharacters = [.. characters];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allowUsualTextFormatting">If to allow usual text formatting characters (tabulator and line break)</param>
        /// <param name="useUnicode">If to use Unicode (otherwise 7bit ASCII)</param>
        /// <param name="additionalDenied">Additional denied characters</param>
        public DenyCharactersAttribute(bool allowUsualTextFormatting = false, bool useUnicode = true, params char[] additionalDenied) : base()
        {
            if (additionalDenied.Length > 0)
            {
                HashSet<char> characters = allowUsualTextFormatting
                    ? useUnicode
                        ? [.. NonTextFormattingSpecialUnicodeCharacters]
                        : [.. NonTextFormattingSpecialCharacters]
                    : useUnicode
                        ? [.. SpecialUnicodeCharacters]
                        : [.. SpecialCharacters];
                for (int i = 0, len = additionalDenied.Length; i < len; characters.Add(additionalDenied[i]), i++) ;
                Denied = [.. characters];
            }
            else
            {
                Denied = allowUsualTextFormatting
                    ? useUnicode
                        ? NonTextFormattingSpecialUnicodeCharacters
                        : NonTextFormattingSpecialCharacters
                    : useUnicode
                        ? SpecialUnicodeCharacters
                        : SpecialCharacters;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="denied">Denied characters</param>
        public DenyCharactersAttribute(params char[] denied) : base() => Denied = [.. denied.Distinct()];

        /// <summary>
        /// Denied characters
        /// </summary>
        public ImmutableArray<char> Denied { get; }

        /// <summary>
        /// If to deny all control characters (<see cref="char.IsControl(char)"/>)
        /// </summary>
        public bool DenyAllControl { get; set; }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is string str)
            {
                ImmutableArray<char> denied = Denied;
                char c;
                for (int i = 0, len = str.Length, j, len2 = denied.Length; i < len; i++)
                {
                    c = str[i];
                    if (DenyAllControl && char.IsControl(c))
                        return this.CreateValidationResult($"Found denied character #{(int)c} at index #{i}", validationContext);
                    for (j = 0; j < len2; j++)
                        if (c == denied[j])
                            return this.CreateValidationResult($"Found denied character #{(int)c} at index #{i}", validationContext);
                }
            }
            else if (value is char chr)
            {
                if (Denied.Contains(chr) || (DenyAllControl && char.IsControl(chr)))
                    return this.CreateValidationResult($"Denied character #{(int)chr}", validationContext);
            }
            else if (value is IList<char> list)
            {
                ImmutableArray<char> denied = Denied;
                char c;
                for (int i = 0, len = list.Count, j, len2 = denied.Length; i < len; i++)
                {
                    c = list[i];
                    if (DenyAllControl && char.IsControl(c))
                        return this.CreateValidationResult($"Found denied character #{(int)c} at index #{i}", validationContext);
                    for (j = 0; j < len2; j++)
                        if (c == denied[j])
                            return this.CreateValidationResult($"Found denied character #{(int)c} at index #{i}", validationContext);
                }
            }
            else
            {
                return this.CreateValidationResult($"{typeof(string)}, {typeof(char)} or {typeof(IList<char>)} expected", validationContext);
            }
            return null;
        }
    }
}
