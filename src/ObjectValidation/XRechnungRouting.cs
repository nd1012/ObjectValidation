using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XRechnung route validation
    /// </summary>
    public static class XRechnungRouting
    {
        /// <summary>
        /// Checksum characters
        /// </summary>
        private const string CHECKSUM_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = new(@"[^\d|A-Z|-]", RegexOptions.Compiled);
        /// <summary>
        /// Syntax regular expression
        /// </summary>
        private static readonly Regex Syntax = new(@"^\d{2}(\d(\d{2}(\d{3})?)?)?-[A-Z|\d]{1,30}-\d{2}$", RegexOptions.Compiled);
        /// <summary>
        /// Checksum calculation regular expression
        /// </summary>
        private static readonly Regex ChecksumCalculation = new(@"[A-Z]", RegexOptions.Compiled);
        /// <summary>
        /// Checksum normalizing regular expression
        /// </summary>
        private static readonly Regex ChecksumNormalizing = new(@"[^\d|A-Z]", RegexOptions.Compiled);

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
    }
}
