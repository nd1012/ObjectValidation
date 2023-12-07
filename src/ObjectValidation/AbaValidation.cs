using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN validation
    /// </summary>
    public static partial class AbaValidation
    {
        /// <summary>
        /// MICR format normalizing regular expression
        /// </summary>
        private static readonly Regex NormalizingMicr = NormalizingMicr_Generated();
        /// <summary>
        /// Fraction format normalizing regular expression
        /// </summary>
        private static readonly Regex NormalizingFraction = NormalizingFraction_Generated();
        /// <summary>
        /// MICR syntax validating regular expression (<c>$1</c> is the FED routing symbol, <c>$2</c> the ABA institution identifier, and <c>$3</c> the check digit)
        /// </summary>
        private static readonly Regex MicrSyntax = MicrSyntax_Generated();
        /// <summary>
        /// Fraction syntax validating regular expression (<c>$1</c> is the ABA prefix, <c>$2</c> the ABA institution identifier, and <c>$3</c> the FED routing symbol)
        /// </summary>
        private static readonly Regex FractionSyntax = FractionSyntax_Generated();

        /// <summary>
        /// Validate an ABA RTN
        /// </summary>
        /// <param name="aba">ABA RTN</param>
        /// <param name="format">Supported format flags</param>
        /// <returns>Valid?</returns>
        public static bool ValidateAbaRtn(string aba, AbaFormats format = AbaFormats.All)
        {
            if (format == AbaFormats.All) format = GetAbaFormat(aba);
            switch (format)
            {
                case AbaFormats.MICR:
                    if (!MicrSyntax.IsMatch(aba)) return false;
                    if (!ValidateFed(int.Parse(aba[..2]))) return false;
                    if (((int.Parse(aba[..1]) + int.Parse(aba.Substring(3, 1)) + int.Parse(aba.Substring(6, 1))) * 3 +
                        (int.Parse(aba.Substring(1, 1)) + int.Parse(aba.Substring(4, 1)) + int.Parse(aba.Substring(7, 1))) * 7 +
                        (int.Parse(aba.Substring(2, 1)) + int.Parse(aba.Substring(5, 1)) + int.Parse(aba.Substring(8, 1))) * 1) % 10 != 0)
                        return false;
                    break;
                case AbaFormats.Fraction:
                    if (!FractionSyntax.IsMatch(aba)) return false;
                    string[] parts = FractionSyntax.Replace(aba, "$1\t$2\t$3").Split('\t');
                    if (!ValidateFed(int.Parse(parts[2][..2]))) return false;
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Normalize an ABA RTN
        /// </summary>
        /// <param name="aba">ABA RTN</param>
        /// <param name="format">Format</param>
        /// <returns>Normalized ABA RTN, or the original value, if the format wasn't given and couldn't be determined</returns>
        public static string Normalize(string aba, AbaFormats format = AbaFormats.All)
        {
            if (format == AbaFormats.All) format = GetAbaFormat(aba);
            return format switch
            {
                AbaFormats.MICR => NormalizingMicr.Replace(aba, string.Empty),
                AbaFormats.Fraction => NormalizingFraction.Replace(aba, string.Empty),
                _ => aba
            };
        }

        /// <summary>
        /// Get the ABA RTN format
        /// </summary>
        /// <param name="aba">ABA RTN</param>
        /// <returns>Format (<see cref="AbaFormats.None"/> is an error with the given ABA RTN!)</returns>
        public static AbaFormats GetAbaFormat(string aba)
        {
            if (MicrSyntax.IsMatch(Normalize(aba, AbaFormats.MICR))) return AbaFormats.MICR;
            if (FractionSyntax.IsMatch(Normalize(aba, AbaFormats.Fraction))) return AbaFormats.Fraction;
            return AbaFormats.None;
        }

        /// <summary>
        /// Validate the FED ID
        /// </summary>
        /// <param name="fed">FED ID</param>
        /// <returns>Valid?</returns>
        private static bool ValidateFed(int fed) => (fed > 0 && fed < 13) || (fed > 20 && fed < 37) || (fed > 60 && fed < 73) || fed == 80;

        /// <summary>
        /// MICR format normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d]", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex NormalizingMicr_Generated();

        /// <summary>
        /// Fraction format normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d|\-|/]", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex NormalizingFraction_Generated();

        /// <summary>
        /// MICR syntax validating regular expression (<c>$1</c> is the FED routing symbol, <c>$2</c> the ABA institution identifier, and <c>$3</c> the check digit)
        /// </summary>
        [GeneratedRegex(@"^(\d{4})(\d{4})(\d)$", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex MicrSyntax_Generated();

        /// <summary>
        /// Fraction syntax validating regular expression (<c>$1</c> is the ABA prefix, <c>$2</c> the ABA institution identifier, and <c>$3</c> the FED routing symbol)
        /// </summary>
        [GeneratedRegex(@"^(\d{1,2})\-(\d{4})/?(\d{4})$", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex FractionSyntax_Generated();
    }
}
