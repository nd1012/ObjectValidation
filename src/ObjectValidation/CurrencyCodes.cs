﻿using System.Collections.Frozen;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Currency ISO 4217 codes
    /// </summary>
    public static class CurrencyCodes
    {
        /// <summary>
        /// Known currencies (ISO 4217 code is the key, a currency information object the value)
        /// </summary>
        public static readonly Dictionary<string, Currency> Known = new()
        {
            {"AFN", new("AFN", "971", "Afghani")},
            {"EUR", new("EUR", "978", "Euro")},
            {"ALL", new("ALL", "008", "Lek")},
            {"DZD", new("DZD", "012", "Algerian Dinar")},
            {"USD", new("USD", "840", "US Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"AOA", new("AOA", "973", "Kwanza")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"ARS", new("ARS", "032", "Argentine Peso")},
            {"AMD", new("AMD", "051", "Armenian Dram")},
            {"AWG", new("AWG", "533", "Aruban Florin")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"AZN", new("AZN", "944", "Azerbaijan Manat")},
            {"BSD", new("BSD", "044", "Bahamian Dollar")},
            {"BHD", new("BHD", "048", "Bahraini Dinar", 3)},
            {"BDT", new("BDT", "050", "Taka")},
            {"BBD", new("BBD", "052", "Barbados Dollar")},
            {"BYN", new("BYN", "933", "Belarusian Ruble")},
            {"EUR", new("EUR", "978", "Euro")},
            {"BZD", new("BZD", "084", "Belize Dollar")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"BMD", new("BMD", "060", "Bermudian Dollar")},
            {"INR", new("INR", "356", "Indian Rupee")},
            {"BTN", new("BTN", "064", "Ngultrum")},
            {"BOB", new("BOB", "068", "Boliviano")},
            {"BOV", new("BOV", "984", "Mvdol")},
            {"USD", new("USD", "840", "US Dollar")},
            {"BAM", new("BAM", "977", "Convertible Mark")},
            {"BWP", new("BWP", "072", "Pula")},
            {"NOK", new("NOK", "578", "Norwegian Krone")},
            {"BRL", new("BRL", "986", "Brazilian Real")},
            {"USD", new("USD", "840", "US Dollar")},
            {"BND", new("BND", "096", "Brunei Dollar")},
            {"BGN", new("BGN", "975", "Bulgarian Lev")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"BIF", new("BIF", "108", "Burundi Franc", 0)},
            {"CVE", new("CVE", "132", "Cabo Verde Escudo")},
            {"KHR", new("KHR", "116", "Riel")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC", 0)},
            {"CAD", new("CAD", "124", "Canadian Dollar")},
            {"KYD", new("KYD", "136", "Cayman Islands Dollar")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC")},
            {"CLP", new("CLP", "152", "Chilean Peso", 0)},
            {"CLF", new("CLF", "990", "Unidad de Fomento", 4)},
            {"CNY", new("CNY", "156", "Yuan Renminbi")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"COP", new("COP", "170", "Colombian Peso")},
            {"COU", new("COU", "970", "Unidad de Valor Real")},
            {"KMF", new("KMF", "174", "Comorian Franc", 0)},
            {"CDF", new("CDF", "976", "Congolese Franc")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC")},
            {"NZD", new("NZD", "554", "New Zealand Dollar")},
            {"CRC", new("CRC", "188", "Costa Rican Colon")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"EUR", new("EUR", "978", "Euro")},
            {"CUP", new("CUP", "192", "Cuban Peso")},
            {"CUC", new("CUC", "931", "Peso Convertible")},
            {"ANG", new("ANG", "532", "Netherlands Antillean Guilder")},
            {"EUR", new("EUR", "978", "Euro")},
            {"CZK", new("CZK", "203", "Czech Koruna")},
            {"DKK", new("DKK", "208", "Danish Krone")},
            {"DJF", new("DJF", "262", "Djibouti Franc", 0)},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"DOP", new("DOP", "214", "Dominican Peso")},
            {"USD", new("USD", "840", "US Dollar")},
            {"EGP", new("EGP", "818", "Egyptian Pound")},
            {"SVC", new("SVC", "222", "El Salvador Colon")},
            {"USD", new("USD", "840", "US Dollar")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC")},
            {"ERN", new("ERN", "232", "Nakfa")},
            {"EUR", new("EUR", "978", "Euro")},
            {"SZL", new("SZL", "748", "Lilangeni")},
            {"ETB", new("ETB", "230", "Ethiopian Birr")},
            {"EUR", new("EUR", "978", "Euro")},
            {"FKP", new("FKP", "238", "Falkland Islands Pound")},
            {"DKK", new("DKK", "208", "Danish Krone")},
            {"FJD", new("FJD", "242", "Fiji Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"EUR", new("EUR", "978", "Euro")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XPF", new("XPF", "953", "CFP Franc")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XAF", new("XAF", "950", "CFA Franc BEAC")},
            {"GMD", new("GMD", "270", "Dalasi")},
            {"GEL", new("GEL", "981", "Lari")},
            {"EUR", new("EUR", "978", "Euro")},
            {"GHS", new("GHS", "936", "Ghana Cedi")},
            {"GIP", new("GIP", "292", "Gibraltar Pound")},
            {"EUR", new("EUR", "978", "Euro")},
            {"DKK", new("DKK", "208", "Danish Krone")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"USD", new("USD", "840", "US Dollar")},
            {"GTQ", new("GTQ", "320", "Quetzal")},
            {"GBP", new("GBP", "826", "Pound Sterling")},
            {"GNF", new("GNF", "324", "Guinean Franc", 0)},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"GYD", new("GYD", "328", "Guyana Dollar")},
            {"HTG", new("HTG", "332", "Gourde")},
            {"USD", new("USD", "840", "US Dollar")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"HNL", new("HNL", "340", "Lempira")},
            {"HKD", new("HKD", "344", "Hong Kong Dollar")},
            {"HUF", new("HUF", "348", "Forint")},
            {"ISK", new("ISK", "352", "Iceland Krona", 0)},
            {"INR", new("INR", "356", "Indian Rupee")},
            {"IDR", new("IDR", "360", "Rupiah")},
            {"XDR", new("XDR", "960", "SDR (Special Drawing Right)", 0)},
            {"IRR", new("IRR", "364", "Iranian Rial")},
            {"IQD", new("IQD", "368", "Iraqi Dinar", 3)},
            {"EUR", new("EUR", "978", "Euro")},
            {"GBP", new("GBP", "826", "Pound Sterling")},
            {"ILS", new("ILS", "376", "New Israeli Sheqel")},
            {"EUR", new("EUR", "978", "Euro")},
            {"JMD", new("JMD", "388", "Jamaican Dollar")},
            {"JPY", new("JPY", "392", "Yen", 0)},
            {"GBP", new("GBP", "826", "Pound Sterling")},
            {"JOD", new("JOD", "400", "Jordanian Dinar", 3)},
            {"KZT", new("KZT", "398", "Tenge")},
            {"KES", new("KES", "404", "Kenyan Shilling")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"KPW", new("KPW", "408", "North Korean Won")},
            {"KRW", new("KRW", "410", "Won", 0)},
            {"KWD", new("KWD", "414", "Kuwaiti Dinar", 3)},
            {"KGS", new("KGS", "417", "Som")},
            {"LAK", new("LAK", "418", "Lao Kip")},
            {"EUR", new("EUR", "978", "Euro")},
            {"LBP", new("LBP", "422", "Lebanese Pound")},
            {"LSL", new("LSL", "426", "Loti")},
            {"ZAR", new("ZAR", "710", "Rand")},
            {"LRD", new("LRD", "430", "Liberian Dollar")},
            {"LYD", new("LYD", "434", "Libyan Dinar", 3)},
            {"CHF", new("CHF", "756", "Swiss Franc")},
            {"EUR", new("EUR", "978", "Euro")},
            {"EUR", new("EUR", "978", "Euro")},
            {"MOP", new("MOP", "446", "Pataca")},
            {"MKD", new("MKD", "807", "Denar")},
            {"MGA", new("MGA", "969", "Malagasy Ariary")},
            {"MWK", new("MWK", "454", "Malawi Kwacha")},
            {"MYR", new("MYR", "458", "Malaysian Ringgit")},
            {"MVR", new("MVR", "462", "Rufiyaa")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO", 0)},
            {"EUR", new("EUR", "978", "Euro")},
            {"USD", new("USD", "840", "US Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"MRU", new("MRU", "929", "Ouguiya")},
            {"MUR", new("MUR", "480", "Mauritius Rupee")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XUA", new("XUA", "965", "ADB Unit of Account", 0)},
            {"MXN", new("MXN", "484", "Mexican Peso")},
            {"MXV", new("MXV", "979", "Mexican Unidad de Inversion (UDI)")},
            {"USD", new("USD", "840", "US Dollar")},
            {"MDL", new("MDL", "498", "Moldovan Leu")},
            {"EUR", new("EUR", "978", "Euro")},
            {"MNT", new("MNT", "496", "Tugrik")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"MAD", new("MAD", "504", "Moroccan Dirham")},
            {"MZN", new("MZN", "943", "Mozambique Metical")},
            {"MMK", new("MMK", "104", "Kyat")},
            {"NAD", new("NAD", "516", "Namibia Dollar")},
            {"ZAR", new("ZAR", "710", "Rand")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"NPR", new("NPR", "524", "Nepalese Rupee")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XPF", new("XPF", "953", "CFP Franc", 0)},
            {"NZD", new("NZD", "554", "New Zealand Dollar")},
            {"NIO", new("NIO", "558", "Cordoba Oro")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"NGN", new("NGN", "566", "Naira")},
            {"NZD", new("NZD", "554", "New Zealand Dollar")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"USD", new("USD", "840", "US Dollar")},
            {"NOK", new("NOK", "578", "Norwegian Krone")},
            {"OMR", new("OMR", "512", "Rial Omani", 3)},
            {"PKR", new("PKR", "586", "Pakistan Rupee")},
            {"USD", new("USD", "840", "US Dollar")},
            {"PAB", new("PAB", "590", "Balboa")},
            {"USD", new("USD", "840", "US Dollar")},
            {"PGK", new("PGK", "598", "Kina")},
            {"PYG", new("PYG", "600", "Guarani", 0)},
            {"PEN", new("PEN", "604", "Sol")},
            {"PHP", new("PHP", "608", "Philippine Peso")},
            {"NZD", new("NZD", "554", "New Zealand Dollar")},
            {"PLN", new("PLN", "985", "Zloty")},
            {"EUR", new("EUR", "978", "Euro")},
            {"USD", new("USD", "840", "US Dollar")},
            {"QAR", new("QAR", "634", "Qatari Rial")},
            {"EUR", new("EUR", "978", "Euro")},
            {"RON", new("RON", "946", "Romanian Leu")},
            {"RUB", new("RUB", "643", "Russian Ruble")},
            {"RWF", new("RWF", "646", "Rwanda Franc", 0)},
            {"EUR", new("EUR", "978", "Euro")},
            {"SHP", new("SHP", "654", "Saint Helena Pound")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"EUR", new("EUR", "978", "Euro")},
            {"EUR", new("EUR", "978", "Euro")},
            {"XCD", new("XCD", "951", "East Caribbean Dollar")},
            {"WST", new("WST", "882", "Tala")},
            {"EUR", new("EUR", "978", "Euro")},
            {"STN", new("STN", "930", "Dobra")},
            {"SAR", new("SAR", "682", "Saudi Riyal")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"RSD", new("RSD", "941", "Serbian Dinar")},
            {"SCR", new("SCR", "690", "Seychelles Rupee")},
            {"SLL", new("SLL", "694", "Leone")},
            {"SLE", new("SLE", "925", "Leone")},
            {"SGD", new("SGD", "702", "Singapore Dollar")},
            {"ANG", new("ANG", "532", "Netherlands Antillean Guilder")},
            {"XSU", new("XSU", "994", "Sucre", 0)},
            {"EUR", new("EUR", "978", "Euro")},
            {"EUR", new("EUR", "978", "Euro")},
            {"SBD", new("SBD", "090", "Solomon Islands Dollar")},
            {"SOS", new("SOS", "706", "Somali Shilling")},
            {"ZAR", new("ZAR", "710", "Rand")},
            {"SSP", new("SSP", "728", "South Sudanese Pound")},
            {"EUR", new("EUR", "978", "Euro")},
            {"LKR", new("LKR", "144", "Sri Lanka Rupee")},
            {"SDG", new("SDG", "938", "Sudanese Pound")},
            {"SRD", new("SRD", "968", "Surinam Dollar")},
            {"NOK", new("NOK", "578", "Norwegian Krone")},
            {"SEK", new("SEK", "752", "Swedish Krona")},
            {"CHF", new("CHF", "756", "Swiss Franc")},
            {"CHE", new("CHE", "947", "WIR Euro")},
            {"CHW", new("CHW", "948", "WIR Franc")},
            {"SYP", new("SYP", "760", "Syrian Pound")},
            {"TWD", new("TWD", "901", "New Taiwan Dollar")},
            {"TJS", new("TJS", "972", "Somoni")},
            {"TZS", new("TZS", "834", "Tanzanian Shilling")},
            {"THB", new("THB", "764", "Baht")},
            {"USD", new("USD", "840", "US Dollar")},
            {"XOF", new("XOF", "952", "CFA Franc BCEAO")},
            {"NZD", new("NZD", "554", "New Zealand Dollar")},
            {"TOP", new("TOP", "776", "Pa’anga")},
            {"TTD", new("TTD", "780", "Trinidad and Tobago Dollar")},
            {"TND", new("TND", "788", "Tunisian Dinar", 3)},
            {"TRY", new("TRY", "949", "Turkish Lira")},
            {"TMT", new("TMT", "934", "Turkmenistan New Manat")},
            {"USD", new("USD", "840", "US Dollar")},
            {"AUD", new("AUD", "036", "Australian Dollar")},
            {"UGX", new("UGX", "800", "Uganda Shilling", 0)},
            {"UAH", new("UAH", "980", "Hryvnia")},
            {"AED", new("AED", "784", "UAE Dirham")},
            {"GBP", new("GBP", "826", "Pound Sterling")},
            {"USD", new("USD", "840", "US Dollar")},
            {"USD", new("USD", "840", "US Dollar")},
            {"USN", new("USN", "997", "US Dollar (Next day)")},
            {"UYU", new("UYU", "858", "Peso Uruguayo")},
            {"UYI", new("UYI", "940", "Uruguay Peso en Unidades Indexadas (UI)", 0)},
            {"UYW", new("UYW", "927", "Unidad Previsional", 4)},
            {"UZS", new("UZS", "860", "Uzbekistan Sum")},
            {"VUV", new("VUV", "548", "Vatu", 0)},
            {"VES", new("VES", "928", "Bolívar Soberano")},
            {"VED", new("VED", "926", "Bolívar Soberano")},
            {"VND", new("VND", "704", "Dong", 0)},
            {"USD", new("USD", "840", "US Dollar")},
            {"USD", new("USD", "840", "US Dollar")},
            {"XPF", new("XPF", "953", "CFP Franc")},
            {"MAD", new("MAD", "504", "Moroccan Dirham")},
            {"YER", new("YER", "886", "Yemeni Rial")},
            {"ZMW", new("ZMW", "967", "Zambian Kwacha")},
            {"ZWL", new("ZWL", "932", "Zimbabwe Dollar")},
            {"XBA", new("XBA", "955", "Bond Markets Unit European Composite Unit (EURCO)", 0)},
            {"XBB", new("XBB", "956", "Bond Markets Unit European Monetary Unit (E.M.U.-6)", 0)},
            {"XBC", new("XBC", "957", "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", 0)},
            {"XBD", new("XBD", "958", "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", 0)},
            {"XAU", new("XAU", "959", "Gold", 0)},
            {"XPD", new("XPD", "964", "Palladium", 0)},
            {"XPT", new("XPT", "962", "Platinum", 0)},
            {"XAG", new("XAG", "961", "Silver", 0)},
        };

        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks>
        /// Constructor
        /// </remarks>
        /// <param name="code">Code</param>
        /// <param name="numericCode">Numeric code</param>
        /// <param name="name">Name</param>
        /// <param name="minorUnit">Minor unit</param>
        public record class Currency(string code, string numericCode, string name, int minorUnit = 2)
        {
            /// <summary>
            /// Factor
            /// </summary>
            protected int? _Factor = null;

            /// <summary>
            /// Code
            /// </summary>
            public string Code { get; set; } = code;

            /// <summary>
            /// Numeric code
            /// </summary>
            public string NumericCode { get; set; } = numericCode;

            /// <summary>
            /// Name
            /// </summary>
            public string Name { get; set; } = name;

            /// <summary>
            /// Minor unit
            /// </summary>
            public int MinorUnit { get; set; } = minorUnit;

            /// <summary>
            /// Factor
            /// </summary>
            public virtual int Factor => _Factor ??= (int)Math.Pow(10, MinorUnit);

            /// <summary>
            /// Validate a value
            /// </summary>
            /// <param name="value">Value</param>
            /// <returns>Valid?</returns>
            public virtual bool Validate(decimal value) => Factor < 1 ? Math.Round(value) == value : Math.Round(value * Factor) / Factor == value;
        }
    }
}
