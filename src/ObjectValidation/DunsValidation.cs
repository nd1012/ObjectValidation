using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// D-U-N-S number validation
    /// </summary>
    public static partial class DunsValidation
    {
        /// <summary>
        /// D-U-N-S syntax validating regular expression
        /// </summary>
        private static readonly Regex DunsSyntax = DunsSyntax_Generated();
        /// <summary>
        /// D-U-N-S syntax normalization regular expression
        /// </summary>
        private static readonly Regex DunsNormalization = DunsNormalization_Generated();

        /// <summary>
        /// Validate a D-U-N-S number
        /// </summary>
        /// <param name="duns">D-U-N-S number</param>
        /// <returns>Valid?</returns>
        public static bool ValidateDuns(ReadOnlySpan<char> duns) => DunsSyntax.IsMatch(duns);

        /// <summary>
        /// Normalize a valid D-U-N-S number to 13 digits
        /// </summary>
        /// <param name="duns">D-U-N-S number</param>
        /// <returns>Normalized D-U-N-S number</returns>
        public static string Normalize(string duns)
        {
            duns = DunsNormalization.Replace(duns, string.Empty);
            return duns.Length switch
            {
                7 => $"00{duns}0000",
                8 => $"0{duns}0000",
                9 => $"{duns}0000",
                _ => duns
            };
        }

        /// <summary>
        /// D-U-N-S syntax validating regular expression
        /// </summary>
        [GeneratedRegex(@"^\d{13}$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex DunsSyntax_Generated();

        /// <summary>
        /// D-U-N-S syntax normalization regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d]", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex DunsNormalization_Generated();
    }
}
