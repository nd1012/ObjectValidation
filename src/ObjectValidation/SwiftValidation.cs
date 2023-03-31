using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// SWIFT (ISO 13616/9362) validation helper
    /// </summary>
    public static class SwiftValidation
    {
        /// <summary>
        /// IBAN checksum calculation characters
        /// </summary>
        private const string IBAN_CHECKSUM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// IBAN checksum parser normalization replacement
        /// </summary>
        private const string IBAN_CHECKSUM_PARSER = "$3$4$1";

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = new(@"[\d|A-Z]", RegexOptions.Singleline | RegexOptions.Compiled);
        /// <summary>
        /// IBAN syntax regular expression (<c>$1</c> is the country, <c>$2</c> the checksum, <c>$3</c> the bank ID and <c>$4</c> the account ID)
        /// </summary>
        private static readonly Regex IbanSyntax = new(@"^([A-Z]{2})(\d{2})(\d{8})(\d{10})$", RegexOptions.Singleline | RegexOptions.Compiled);
        /// <summary>
        /// IBAN checksum calculation regular expression
        /// </summary>
        private static readonly Regex IbanChecksum = new(@"[A-Z]", RegexOptions.Singleline | RegexOptions.Compiled);
        /// <summary>
        /// BIC syntax regular expression
        /// </summary>
        private static readonly Regex BicSyntax = new(@"^[A-Z|\d]{4}[A-Z]{2}[A-Z|\d]{2}([A-Z|\d]{3})?$", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Split an IBAN (ISO 13616) into country, checksum, bank ID and account ID
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <returns>Country, checksum, bank ID and account ID</returns>
        public static (string CountryCode, string Checksum, string BankId, string AccountId) SplitIban(string iban)
        {
            if (!IbanSyntax.IsMatch(iban)) throw new ArgumentException("Invalid IBAN", nameof(iban));
            return (iban[..2], iban.Substring(2, 2), iban.Substring(4, 8), iban[12..]);
        }

        /// <summary>
        /// Validate an IBAN (ISO 13616)
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <returns>Valid?</returns>
        public static bool ValidateIban(string iban)
            => IbanSyntax.IsMatch(iban) &&
                CountryCodes.Known.ContainsKey(iban[..2]) &&
                iban.Substring(2, 2) ==
                (
                    98 -
                    (
                        decimal.Parse(
                            IbanChecksum.Replace($"{IbanSyntax.Replace(iban, IBAN_CHECKSUM_PARSER)}00", (m) => (IBAN_CHECKSUM_CHARS.IndexOf(m.Groups[0].Value) + 10).ToString())
                        ) % 97
                    )
                ).ToString().PadLeft(2, '0');

        /// <summary>
        /// Validate a BIC (ISO 9362)
        /// </summary>
        /// <param name="bic">BIC</param>
        /// <returns>Valid?</returns>
        public static bool ValidateBic(string bic) => BicSyntax.IsMatch(bic) && CountryCodes.Known.ContainsKey(bic.Substring(4, 2));

        /// <summary>
        /// Normalize an IBAN (ISO 13616) or BIC (ISO 9362)
        /// </summary>
        /// <param name="str">IBAN/BIC</param>
        /// <returns>Normalized value</returns>
        public static string Normalize(string str) => Normalizing.Replace(str.ToUpper(), string.Empty);
    }
}
