using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XRechnung route validation
    /// </summary>
    public static partial class XRechnungRouting
    {
        /// <summary>
        /// Checksum characters
        /// </summary>
        private const string CHECKSUM_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = Normalizing_Generated();
        /// <summary>
        /// Syntax regular expression
        /// </summary>
        private static readonly Regex Syntax = Syntax_Generated();
        /// <summary>
        /// Checksum calculation regular expression
        /// </summary>
        private static readonly Regex ChecksumCalculation = ChecksumCalculation_Generated();
        /// <summary>
        /// Checksum normalizing regular expression
        /// </summary>
        private static readonly Regex ChecksumNormalizing = ChecksumNormalizing_Generated();

        /// <summary>
        /// Validate a XRechnung route
        /// </summary>
        /// <param name="route">Route</param>
        /// <returns>Valid?</returns>
        public static bool Validate(string route)
            => Syntax.IsMatch(route) &&
                decimal.Parse(ChecksumCalculation.Replace(ChecksumNormalizing.Replace(route, string.Empty), m => (CHECKSUM_CHARACTERS.IndexOf(m.Groups[0].Value) + 10).ToString())) % 97 == 1;

        /// <summary>
        /// Normalize a XRechnung route
        /// </summary>
        /// <param name="route">Route</param>
        /// <returns>Normalized route</returns>
        public static string Normalize(string route) => Normalizing.Replace(route.ToUpper(), string.Empty);

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d|A-Z|-]", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex Normalizing_Generated();

        /// <summary>
        /// Syntax regular expression
        /// </summary>
        [GeneratedRegex(@"^\d{2}(\d(\d{2}(\d{3})?)?)?-[A-Z|\d]{1,30}-\d{2}$", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex Syntax_Generated();

        /// <summary>
        /// Checksum calculation regular expression
        /// </summary>
        [GeneratedRegex(@"[A-Z]", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex ChecksumCalculation_Generated();

        /// <summary>
        /// Checksum normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d|A-Z]", RegexOptions.Compiled | RegexOptions.Singleline)]
        private static partial Regex ChecksumNormalizing_Generated();
    }
}
