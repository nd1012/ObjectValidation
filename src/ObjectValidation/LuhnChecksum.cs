﻿using System.Text.RegularExpressions;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Luhn checksum validation
    /// </summary>
    public static partial class LuhnChecksum
    {
        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        private static readonly Regex Normalizing = Normalizing_Generated();

        /// <summary>
        /// Validate the Luhn checksum of a value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Valid?</returns>
        public static bool Validate(string value)
            => value.Select((c, i) => (c - '0') << ((value.Length - i - 1) & 1)).Sum(n => n > 9 ? n - 9 : n) % 10 == 0;

        /// <summary>
        /// Normalize a value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Normalized value</returns>
        public static string Normalize(string value) => Normalizing.Replace(value, string.Empty);

        /// <summary>
        /// Normalizing regular expression
        /// </summary>
        [GeneratedRegex(@"[^\d]", RegexOptions.Compiled | RegexOptions.Singleline, 3000)]
        private static partial Regex Normalizing_Generated();
    }
}
