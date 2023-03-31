namespace wan24.ObjectValidation
{
    /// <summary>
    /// ABA RTN formats
    /// </summary>
    [Flags]
    public enum AbaFormats : int
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// MICR (magnetic ink character recognition) XXXXYYYYC
        /// </summary>
        MICR = 1,
        /// <summary>
        /// Fraction PP-YYYY/XXXX
        /// </summary>
        Fraction = 2,
        /// <summary>
        /// All formats
        /// </summary>
        All = MICR | Fraction
    }
}
