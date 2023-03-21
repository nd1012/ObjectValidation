namespace wan24.ObjectValidation
{
    /// <summary>
    /// Currency formats
    /// </summary>
    [Flags]
    public enum CurrencyFormats : int
    {
        /// <summary>
        /// Alphabetic code (3 characters uppercase)
        /// </summary>
        AlphabeticCode = 1,
        /// <summary>
        /// Numeric code (3 digits)
        /// </summary>
        NumericCode = 2
    }
}
