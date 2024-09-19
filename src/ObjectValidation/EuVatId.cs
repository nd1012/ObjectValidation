using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// European VAT ID validation
    /// </summary>
    public static partial class EuVatId
    {
        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = Normalizing_Generated();

        /// <summary>
        /// VAT ID syntax regular expressions
        /// </summary>
        private static readonly FrozenDictionary<string, Regex> Syntax = new Dictionary<string, Regex>()
        {
            {"AT", AT_Generated()},
            {"BE", BE_Generated()},
            {"BG", BG_Generated()},
            {"CY", CY_Generated()},
            {"CZ", CZ_Generated()},
            {"DE", DE_Generated()},
            {"DK", DK_Generated()},
            {"EE", EE_Generated()},
            {"ES", ES_Generated()},
            {"FI", FI_Generated()},
            {"FR", FR_Generated()},
            {"GR", GR_Generated()},
            {"HR", HR_Generated()},
            {"HU", HU_Generated()},
            {"IE", IE_Generated()},
            {"IT", IT_Generated()},
            {"LT", LT_Generated()},
            {"LU", LU_Generated()},
            {"LV", LV_Generated()},
            {"MT", MT_Generated()},
            {"NL", NL_Generated()},
            {"PL", PL_Generated()},
            {"PT", PT_Generated()},
            {"RO", RO_Generated()},
            {"SE", SE_Generated()},
            {"SI", SI_Generated()},
            {"SK", SK_Generated()},
            {"XI", XI_Generated()}// North Ireland, since Brexit
        }.ToFrozenDictionary();

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

        /// <summary>
        /// VAT ID syntax regular expression for AT
        /// </summary>
        [GeneratedRegex(@"^(ATU)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex AT_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for BE
        /// </summary>
        [GeneratedRegex(@"^(BE)(\d{10})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex BE_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for BG
        /// </summary>
        [GeneratedRegex(@"^(BG)(\d{9,10})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex BG_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for CY
        /// </summary>
        [GeneratedRegex(@"^(CY)(\d{8}[A-Z])$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex CY_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for CZ
        /// </summary>
        [GeneratedRegex(@"^(CZ)(\d{8,10})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex CZ_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for DE
        /// </summary>
        [GeneratedRegex(@"^(DE)(\d{9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex DE_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for DK
        /// </summary>
        [GeneratedRegex(@"^(DK)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex DK_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for EE
        /// </summary>
        [GeneratedRegex(@"^(EE)(\d{9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex EE_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for ES
        /// </summary>
        [GeneratedRegex(@"^(ES)([A-Z|\d]\d{7}[A-Z|\d])$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex ES_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for FI
        /// </summary>
        [GeneratedRegex(@"^(FI)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex FI_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for FR
        /// </summary>
        [GeneratedRegex(@"^(FR)([A-Z|\d]{2}\d{9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex FR_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for GR
        /// </summary>
        [GeneratedRegex(@"^(GR)(\d{9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex GR_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for HR
        /// </summary>
        [GeneratedRegex(@"^(HR)(\d{11})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex HR_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for HU
        /// </summary>
        [GeneratedRegex(@"^(HU)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex HU_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for IE
        /// </summary>
        [GeneratedRegex(@"^(IE)((\d[A-Z|\d]\d{5}[A-Z])|(\d{7}[A-W][A-I]))$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex IE_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for IT
        /// </summary>
        [GeneratedRegex(@"^(IT)(\d{11})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex IT_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for LT
        /// </summary>
        [GeneratedRegex(@"^(LT)(\d{9}|\d{12})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex LT_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for LU
        /// </summary>
        [GeneratedRegex(@"^(LU)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex LU_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for LV
        /// </summary>
        [GeneratedRegex(@"^(LV)(\d{11})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex LV_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for MT
        /// </summary>
        [GeneratedRegex(@"^(MT)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex MT_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for NL
        /// </summary>
        [GeneratedRegex(@"^(NL)([\d|A-Z|\+|\*]{12})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex NL_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for PL
        /// </summary>
        [GeneratedRegex(@"^(PL)(\d{10})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex PL_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for PT
        /// </summary>
        [GeneratedRegex(@"^(PT)(\d{9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex PT_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for RO
        /// </summary>
        [GeneratedRegex(@"^(RO)([1-9]\d{0,9})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex RO_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for SE
        /// </summary>
        [GeneratedRegex(@"^(SE)(\d{10}01)$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex SE_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for SI
        /// </summary>
        [GeneratedRegex(@"^(SI)(\d{8})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex SI_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for SK
        /// </summary>
        [GeneratedRegex(@"^(SK)(\d{10})$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex SK_Generated();

        /// <summary>
        /// VAT ID syntax regular expression for XI
        /// </summary>
        [GeneratedRegex(@"^(XI)(\d{9}|\d{12}|(GD\d{3})|(HA\d{3}))$", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex XI_Generated();

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d|A-Z]", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex Normalizing_Generated();
    }
}
