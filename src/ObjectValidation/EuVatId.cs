using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// European VAT ID validation
    /// </summary>
    public static class EuVatId
    {
        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = new(@"[^\d|A-Z]", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// VAT ID syntax regular expressions
        /// </summary>
        private static readonly Dictionary<string, Regex> Syntax = new()
        {
            {"BE", new(@"^(BE)(\d{10})$",RegexOptions.Singleline|RegexOptions.Compiled) },
            {"BG", new(@"^(BG)(\d{9,10})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"DK", new(@"^(DK)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"DE", new(@"^(DE)(\d{9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"EE", new(@"^(EE)(\d{9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"FI", new(@"^(FI)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"FR", new(@"^(FR)([A-Z|\d]{2}\d{9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"GR", new(@"^(GR)(\d{9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"IT", new(@"^(IT)(\d{11})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"IE", new(@"^(IE)((\d[A-Z|\d]\d{5}[A-Z])|(\d{7}[A-W][A-I]))$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"HR", new(@"^(HR)(\d{11})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"LV", new(@"^(LV)(\d{11})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"LT", new(@"^(LT)(\d{9}|\d{12})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"LU", new(@"^(LU)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"MT", new(@"^(MT)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"NL", new(@"^(NL)([\d|A-Z|\+|\*]{12})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"AT", new(@"^(ATU)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"PL", new(@"^(PL)(\d{10})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"PT", new(@"^(PT)(\d{9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"RO", new(@"^(RO)([1-9]\d{0,9})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"SE", new(@"^(SE)(\d{10}01)$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"SI", new(@"^(SI)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"SK", new(@"^(SK)(\d{10})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"ES", new(@"^(ES)([A-Z|\d]\d{7}[A-Z|\d])$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"CZ", new(@"^(CZ)(\d{8,10})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"HU", new(@"^(HU)(\d{8})$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"CY", new(@"^(CY)(\d{8}[A-Z])$", RegexOptions.Singleline|RegexOptions.Compiled)},
            {"XI", new(@"^(XI)(\d{9}|\d{12}|(GD\d{3})|(HA\d{3}))$", RegexOptions.Singleline|RegexOptions.Compiled)}// North Ireland, since Brexit
        };

        /// <summary>
        /// Validate a European VAT ID
        /// </summary>
        /// <param name="vatId">VAT ID</param>
        /// <returns>Valid?</returns>
        public static bool Validate(string vatId) => vatId.Length > 2 && Syntax.ContainsKey(vatId[..2]) && Syntax[vatId[..2]].IsMatch(vatId);

        /// <summary>
        /// Normalize a European VAT ID
        /// </summary>
        /// <param name="vatId">VAT ID</param>
        /// <returns>Normalized VAT ID</returns>
        public static string Normalize(string vatId) => Normalizing.Replace(vatId, string.Empty);
    }
}
